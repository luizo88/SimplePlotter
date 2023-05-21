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
        private bool secondY;
        private List<PointObj> gifPoints = new List<PointObj>();
        private List<int> gifKeyIndexes = new List<int>();

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
            this.lineStyle = OxyPlot.LineStyle.Solid;
            foreach (var item in Enum.GetValues(typeof(Enums.Colors)))
            {
                availableColors.Add((Enums.Colors)item);
            }
            this.color = Enums.Colors.Black;
            this.customColor = false;
            this.standardColor = true;
            this.rgb = ColorTemplateController.GetRGBFromColor(color);
            this.rgbDescription = ColorTemplateController.GetRGBDescriptionFromColor(color);
            this.legend = true;
            this.secondY = false;
        }

        public DataSeriesObj(string name, List<double> xPoints, List<double> yPoints, double xScale, double yScale,
            double thick, OxyPlot.LineStyle lineStyle, SimplePlotterMisc.Enums.Colors color, bool customColor, string RGBDescription, bool legend, bool secondY)
        {
            this.name = name;
            this.xScale = xScale;
            this.yScale = yScale;
            updatePointList(xScale, yScale, xPoints, yPoints);
            this.thick = thick;
            foreach (var item in Enum.GetValues(typeof(OxyPlot.LineStyle)))
            {
                availableLineStyles.Add((OxyPlot.LineStyle)item);
            }
            this.lineStyle = lineStyle;
            foreach (var item in Enum.GetValues(typeof(Enums.Colors)))
            {
                availableColors.Add((Enums.Colors)item);
            }
            this.color = color;
            this.CustomColor = customColor;
            this.RGBDescription = RGBDescription;
            this.legend = legend;
            this.secondY = secondY;
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

        public List<double> XPoints
        {
            get
            {
                var linq = from p
                           in points
                           select p.X;
                return linq.ToList();
            }
        }
        public List<double> YPoints
        {
            get
            {
                var linq = from p
                           in points
                           select p.Y;
                return linq.ToList();
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

        public bool SecondY
        {
            get { return secondY; }
            set
            {
                secondY = value;
                NotifyPropertyChanged();
            }
        }

        public List<PointObj> GIFPoints
        {
            get { return gifPoints; }
            set
            {
                gifPoints = value;
                NotifyPropertyChanged();
            }
        }

        public List<int> GIFKeyIndexes
        {
            get { return gifKeyIndexes; }
            set
            {
                gifKeyIndexes = value;
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

        #region PUBLIC METHODS

        public void GenerateGIFPoints(int numberOfFrames, bool interpolateData)
        {
            gifPoints.Clear();
            gifKeyIndexes.Clear();
            if (interpolateData)
            {
                double dx = points.Last().ScaledX / (numberOfFrames - 1);
                int lastIndex = 0;
                for (int i = 0; i < numberOfFrames - 1; i++)
                {
                    //finds the last point of this section
                    double x = points[0].ScaledX + i * dx;
                    int index1 = points.IndexOf(points.Find(p => p.ScaledX > x)) - 1;
                    double x1 = points[index1].ScaledX;
                    double y1 = points[index1].ScaledY;
                    double x2 = points[index1 + 1].ScaledX;
                    double y2 = points[index1 + 1].ScaledY;
                    double y = y1 + (y2 - y1) / (x2 - x1) * (x - x1);
                    //adds the intermediate points
                    for (int j = lastIndex + 1; j < index1 + 1; j++)
                    {
                        gifPoints.Add(new PointObj(points[j].ScaledX, points[j].ScaledY));
                    }
                    //adds the last point
                    gifPoints.Add(new PointObj(x, y));
                    gifKeyIndexes.Add(gifPoints.Count - 1);
                    lastIndex = index1;
                }
                gifPoints.Add(new PointObj(points.Last().ScaledX, points.Last().ScaledY));
                gifKeyIndexes.Add(gifPoints.Count - 1);
            }
            else
            {
                gifPoints.Add(new PointObj(points[0].ScaledX, points[0].ScaledY));
                gifKeyIndexes.Add(0);
                int lastIndex = 0;
                double pointsPerFrame = (double)(points.Count - 1) / (numberOfFrames);
                for (int i = 1; i <= numberOfFrames - 1; i++)
                {
                    int index1 = (int)Math.Round(i * pointsPerFrame);
                    for (int j = lastIndex + 1; j < index1 + 1; j++)
                    {
                        gifPoints.Add(new PointObj(points[j].ScaledX, points[j].ScaledY));
                    }
                    gifKeyIndexes.Add(gifPoints.Count - 1);
                    lastIndex = index1;
                }
            }
        }

        #endregion

    }
}
