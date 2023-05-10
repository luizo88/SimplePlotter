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
    public class DataSeriesController : Auxiliary.PropertyNotify
    {
        private static DataSeriesController instance = new DataSeriesController();
        List<DataSeriesObj> dataSeries = new List<DataSeriesObj>();

        public DataSeriesController() 
        {

        }

        #region EVENTS

        private void onDataSeriesPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Name":
                case "Points":
                case "XScale":
                case "YScale":
                case "Thick":
                case "LineStyle":
                case "RGBDescription":
                case "Legend":
                    NotifyPropertyChanged("NeedToPlotAgain");
                    break;
            }
        }

        #endregion

        #region PROPERTIES

        public static DataSeriesController Instance
        {
            get
            {
                return instance;
            }
        }

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

        #region PUBLIC METHODS

        public void AddDataSeries(string name, List<double> xPoints, List<double> yPoints) 
        {
            dataSeries.Add(new DataSeriesObj(name, xPoints, yPoints));
            //assigns to detect changes
            dataSeries.Last().PropertyChanged += onDataSeriesPropertyChanged;
            NotifyPropertyChanged("DataSeries");
            NotifyPropertyChanged("NeedToPlotAgain");
        }

        public void RemoveDataSeries(DataSeriesObj dataSeriesToRemove)
        {
            dataSeriesToRemove.PropertyChanged -= onDataSeriesPropertyChanged;
            dataSeries.Remove(dataSeriesToRemove);
            NotifyPropertyChanged("DataSeries");
            NotifyPropertyChanged("NeedToPlotAgain");
        }

        public void MoveDataSeriesUp(DataSeriesObj dataSeriesToMove)
        {
            int index = dataSeries.IndexOf(dataSeriesToMove);
            dataSeries.RemoveAt(index);
            dataSeries.Insert(index - 1, dataSeriesToMove);
        }

        public void MoveDataSeriesDown(DataSeriesObj dataSeriesToMove)
        {
            int index = dataSeries.IndexOf(dataSeriesToMove);
            dataSeries.RemoveAt(index);
            dataSeries.Insert(index + 1, dataSeriesToMove);
        }

        #endregion

    }
}
