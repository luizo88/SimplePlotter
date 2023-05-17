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

        /// <summary>
        /// Creates a new point object.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="scaleX">A scale to be applied in x coordinate.</param>
        /// <param name="scaleY">A scale to be applied in y coordinate.</param>
        /// <param name="scaledX">The x coordinate scaled.</param>
        /// <param name="scaledY">The y coordinate scaled.</param>
        public PointObj(double x, double y, double scaleX, double scaleY, double scaledX, double scaledY)
        {
            this.x = x;
            this.y = y;
            this.scaleX = scaleX;
            this.scaleY = scaleY;
            this.scaledX = scaledX;
            this.scaledY = scaledY;
        }

        /// <summary>
        /// Creates a new point object with x and y scales equals to 1.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public PointObj(double x, double y)
        {
            this.x = x;
            this.y = y;
            this.scaleX = 1;
            this.scaleY = 1;
            this.scaledX = x;
            this.scaledY = y;
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
