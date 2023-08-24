using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplePlotterMisc
{
    /// <summary>
    /// A class to control the data series behavior.
    /// </summary>
    public class DataSeriesController : Auxiliary.PropertyNotify
    {
        private static DataSeriesController instance = new DataSeriesController();
        List<DataSeriesObj> dataSeries = new List<DataSeriesObj>();

        public DataSeriesController() 
        {

        }

        #region EVENTS

        /// <summary>
        /// Event used to capture changes inside data series.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onDataSeriesPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(e.PropertyName);
        }

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Gets the one instance of the class.
        /// </summary>
        public static DataSeriesController Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Get or sets the data series list (but don't add series by this way, please use the method of the controller).
        /// </summary>
        public List<DataSeriesObj> DataSeries
        {
            get { return dataSeries; }
            set
            {
                dataSeries = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region PRIVATE METHODS

        private DataSeriesObj ExecuteRamerDouglasPeuckerAlgorithm(DataSeriesObj oritinalDataSeries)
        {
            //https://stackoverflow.com/questions/7980586/how-to-reduce-the-number-of-points-in-a-curve-while-preserving-its-overall-shape
            //https://en.wikipedia.org/wiki/Ramer%E2%80%93Douglas%E2%80%93Peucker_algorithm
            //pendente
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Adds a new data series to the list.
        /// </summary>
        /// <param name="name">The name of the data series.</param>
        /// <param name="xPoints">A list containing the x-coordinates.</param>
        /// <param name="yPoints">A list containing the y-coordinates.</param>
        public void AddDataSeries(string name, List<double> xPoints, List<double> yPoints) 
        {
            dataSeries.Add(new DataSeriesObj(name, xPoints, yPoints));
            //assigns to detect changes
            dataSeries.Last().PropertyChanged += onDataSeriesPropertyChanged;
            NotifyPropertyChanged("DataSeries");
            NotifyPropertyChanged("NeedToPlotAgain");
        }

        /// <summary>
        /// Remove a deta series from the list.
        /// </summary>
        /// <param name="dataSeriesToRemove">The data series to be removed.</param>
        public void RemoveDataSeries(DataSeriesObj dataSeriesToRemove)
        {
            dataSeriesToRemove.PropertyChanged -= onDataSeriesPropertyChanged;
            dataSeries.Remove(dataSeriesToRemove);
            NotifyPropertyChanged("DataSeries");
            NotifyPropertyChanged("NeedToPlotAgain");
        }

        /// <summary>
        /// Moves a specific data series one position up in the list.
        /// </summary>
        /// <param name="dataSeriesToMove">The data series to be moved.</param>
        public void MoveDataSeriesUp(DataSeriesObj dataSeriesToMove)
        {
            int index = dataSeries.IndexOf(dataSeriesToMove);
            dataSeries.RemoveAt(index);
            dataSeries.Insert(index - 1, dataSeriesToMove);
        }

        /// <summary>
        /// Moves a specific data series one position down in the list.
        /// </summary>
        /// <param name="dataSeriesToMove">The data series to be moved.</param>
        public void MoveDataSeriesDown(DataSeriesObj dataSeriesToMove)
        {
            int index = dataSeries.IndexOf(dataSeriesToMove);
            dataSeries.RemoveAt(index);
            dataSeries.Insert(index + 1, dataSeriesToMove);
        }

        /// <summary>
        /// Generates (creates inside each data series) the points to be used to generate a GIF.
        /// </summary>
        /// <param name="numberOfPoints">The total number of points </param>
        /// <param name="interpolateData">The number of points to have in the GIF (in fact, is the number of frames).</param>
        public void GenerateGIFPointsForAllSeries(int numberOfPoints, bool interpolateData)
        {            
            foreach (var item in dataSeries)
            {
                item.GenerateGIFPoints(numberOfPoints, interpolateData);
            }
        }

        #endregion

    }
}
