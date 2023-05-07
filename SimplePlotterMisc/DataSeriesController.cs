using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void AddDataSeries(string name, List<Point> points) 
        {
            dataSeries.Add(new DataSeriesObj(name, points));
            NotifyPropertyChanged("DataSeries");
        }

        public void RemoveDataSeries(DataSeriesObj dataSeriesToRemove)
        {
            dataSeries.Remove(dataSeriesToRemove);
            NotifyPropertyChanged("DataSeries");
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
