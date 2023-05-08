using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplePlotterMisc
{
    public class PointObj : Auxiliary.PropertyNotify
    {
        double x;
        double y;
        double scaleX;
        double scaleY;
        double scaledX;
        double scaledY;

        public PointObj(double x, double y, double scaleX, double scaleY, double scaledX, double scaledY)
        {
            this.x = x;
            this.y = y;
            this.scaleX = scaleX;
            this.scaleY = scaleY;
            this.scaledX = scaledX;
            this.scaledY = scaledY;
        }

        #region PROPERTIES

        public double X
        {
            get { return x; }
            set 
            {
                x = value;
                ScaledX = x * scaleX;
                NotifyPropertyChanged();
            }
        }

        public double Y
        {
            get { return y; }
            set
            {
                y = value;
                ScaledY = y * scaleY;
                NotifyPropertyChanged();
            }
        }

        public double ScaleX
        {
            get { return scaleX; }
            set
            {
                scaleX = value;
                ScaledX = x * scaleX;
                NotifyPropertyChanged();
            }
        }

        public double ScaleY
        {
            get { return scaleY; }
            set
            {
                scaleY = value;
                ScaledY = y * scaleY;
                NotifyPropertyChanged();
            }
        }

        public double ScaledX
        {
            get { return scaledX; }
            set
            {
                scaledX = value;
                NotifyPropertyChanged();
            }
        }

        public double ScaledY
        {
            get { return scaledY; }
            set
            {
                scaledY = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

    }
}
