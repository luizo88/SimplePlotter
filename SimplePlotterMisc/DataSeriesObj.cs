using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplePlotterMisc
{
    public class DataSeriesObj : Auxiliary.PropertyNotify
    {
        string name;
        List<Point> points = new List<Point>();
        double scale;

        public DataSeriesObj(string name, List<Point> points) 
        {
            this.name = name;
            this.points = points;
            this.scale = 1;
        }

        #region PROPERTIES

        public string Name
        {
            get { return name; }
            set 
            {
                name = value;
                NotifyPropertyChanged();
            }
        }

        public List<Point> Points
        {
            get { return points; }
            set 
            {
                points = value;
                NotifyPropertyChanged();
            }
        }

        public double Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

    }
}
