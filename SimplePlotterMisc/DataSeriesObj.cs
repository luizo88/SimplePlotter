using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplePlotterMisc
{
    public class DataSeriesObj : Auxiliary.PropertyNotify
    {
        private string name;
        private List<PointObj> points = new List<PointObj>();
        private double xScale;
        private double yScale;
        private double thick;
        private List<OxyPlot.LineStyle> availableLineStyles = new List<OxyPlot.LineStyle>();
        private OxyPlot.LineStyle lineStyle;
        private List<Enums.Colors> availableColors = new List<Enums.Colors>();
        private Enums.Colors color;
        private bool customColor;
        private bool standardColor;
        private string rgbDescription;
        private Tuple<byte, byte, byte> rgb;
        private bool legend;

        public DataSeriesObj(string name, List<double> xPoints, List<double> yPoints) 
        {
            this.name = name;
            this.xScale = 1;
            this.yScale = 1;
            updatePointList(xScale, yScale, xPoints, yPoints);
            this.thick = 1.5;
            foreach (var item in Enum.GetValues(typeof(OxyPlot.LineStyle)))
            {
                availableLineStyles.Add((OxyPlot.LineStyle)item);
            }
            lineStyle = OxyPlot.LineStyle.Solid;
            foreach (var item in Enum.GetValues(typeof(Enums.Colors)))
            {
                availableColors.Add((Enums.Colors)item);
            }
            color = Enums.Colors.Black;
            customColor = false;
            standardColor = true;
            rgb = ColorTemplateController.GetRGBFromColor(color);
            rgbDescription = ColorTemplateController.GetRGBDescriptionFromColor(color);
            legend = true;
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

        public List<PointObj> Points
        {
            get { return points; }
            set
            {
                points = value;
                NotifyPropertyChanged();
            }
        }

        public double XScale
        {
            get { return xScale; }
            set
            {
                xScale = value;
                updatePointList();
                NotifyPropertyChanged();
            }
        }

        public double YScale
        {
            get { return yScale; }
            set
            {
                yScale = value;
                updatePointList();
                NotifyPropertyChanged();
            }
        }

        public int Length
        {
            get { return points.Count; }
        }

        public double Thick
        {
            get { return thick; }
            set
            {
                thick = value;
                NotifyPropertyChanged();
            }
        }

        public List<OxyPlot.LineStyle> AvailableLineStyles
        {
            get { return availableLineStyles; }
            set
            {
                availableLineStyles = value;
                NotifyPropertyChanged();
            }
        }

        public OxyPlot.LineStyle LineStyle
        {
            get { return lineStyle; }
            set
            {
                lineStyle = value;
                NotifyPropertyChanged();
            }
        }

        public List<Enums.Colors> AvailableColors
        {
            get { return availableColors; }
            set
            {
                availableColors = value;
                NotifyPropertyChanged();
            }
        }

        public Enums.Colors Color
        {
            get { return color; }
            set
            {
                color = value;
                RGB = ColorTemplateController.GetRGBFromColor(color);
                RGBDescription = ColorTemplateController.GetRGBDescriptionFromColor(color);
                NotifyPropertyChanged();
            }
        }

        public OxyColor OxyColor
        {
            get { return OxyColor.FromRgb(rgb.Item1, rgb.Item2, rgb.Item3); }
        }

        public bool CustomColor
        {
            get { return customColor; }
            set
            {
                customColor = value;
                StandardColor = !customColor;
                RGBDescription = ColorTemplateController.GetRGBDescriptionFromColor(color);
                RGB = ColorTemplateController.GetRGBFromRGBDescription(rgbDescription);
                NotifyPropertyChanged();
            }
        }

        public bool StandardColor
        {
            get { return standardColor; }
            set
            {
                standardColor = value;
                NotifyPropertyChanged();
            }
        }

        public string RGBDescription
        {
            get { return rgbDescription; }
            set
            {
                if(ColorTemplateController.ValidateRGBDescription(value))
                {
                    rgbDescription = value;
                    rgb = ColorTemplateController.GetRGBFromRGBDescription(rgbDescription);
                    NotifyPropertyChanged();
                }
            }
        }

        public Tuple<byte, byte, byte> RGB
        {
            get { return rgb; }
            set
            {
                rgb = value;
                NotifyPropertyChanged();
            }
        }

        public bool Legend
        {
            get { return legend; }
            set
            {
                legend = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region PRIVATE METHODS

        private void updatePointList()
        {
            var linqX = from p
                        in points
                        select p.X;
            List<double> xPoints = linqX.ToList();
            var linqY = from p
                        in points
                        select p.Y;
            List<double> yPoints = linqY.ToList();
            updatePointList(xScale, yScale, xPoints, yPoints);
        }

        private void updatePointList(double scaleX, double scaleY, List<double> xPoints, List<double> yPoints)
        {
            points.Clear();
            for (int i = 0; i < xPoints.Count; i++)
            {
                points.Add(new PointObj(xPoints[i], yPoints[i], scaleX, scaleY, xPoints[i] * xScale, yPoints[i] * YScale));
            }
            NotifyPropertyChanged("Points");
        }

        #endregion

    }
}
