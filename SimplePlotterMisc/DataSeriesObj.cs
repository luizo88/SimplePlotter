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
    /// <summary>
    /// A class to represent/controll a data series (a curve).
    /// </summary>
    public class DataSeriesObj : Auxiliary.PropertyNotify
    {
        private string name;
        private List<PointObj> points = new List<PointObj>();
        private double xScale;
        private double yScale;
        private double thick;
        private List<OxyPlot.LineStyle> availableLineStyles = new List<OxyPlot.LineStyle>();
        private OxyPlot.LineStyle lineStyle;
        private List<OxyPlot.MarkerType> availableMarkerTypes = new List<OxyPlot.MarkerType>();
        private OxyPlot.MarkerType markerType;
        private List<Enums.Colors> availableColors = new List<Enums.Colors>();
        private double markerSize;
        private Enums.Colors color;
        private bool customColor;
        private bool standardColor;
        private string rgbDescription;
        private Tuple<byte, byte, byte> rgb;
        private bool legend;
        private bool secondY;
        private List<PointObj> gifPoints = new List<PointObj>();
        private List<int> gifKeyIndexes = new List<int>();

        /// <summary>
        /// Creates a new data series with some default parameters.
        /// </summary>
        /// <param name="name">The name of the data series.</param>
        /// <param name="xPoints">A list containing the x-coordinates.</param>
        /// <param name="yPoints">A list containing the y-coordinates.</param>
        public DataSeriesObj(string name, List<double> xPoints, List<double> yPoints) 
        {
            this.name = name;
            this.xScale = 1;
            this.yScale = 1;
            updatePointList(xScale, yScale, xPoints, yPoints);
            this.thick = GetDefaultDataSeriesThick();
            foreach (var item in Enum.GetValues(typeof(OxyPlot.LineStyle)))
            {
                availableLineStyles.Add((OxyPlot.LineStyle)item);
            }
            this.lineStyle = OxyPlot.LineStyle.Solid;
            foreach (var item in Enum.GetValues(typeof(OxyPlot.MarkerType)))
            {
                if ((OxyPlot.MarkerType)item != MarkerType.Custom)
                {
                    availableMarkerTypes.Add((OxyPlot.MarkerType)item);
                }
            }
            this.markerType = OxyPlot.MarkerType.None;
            this.MarkerSize = GetDefaultDataSeriesMarkerSize();
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

        /// <summary>
        /// Creates a new data series.
        /// </summary>
        /// <param name="name">The name of the data series.</param>
        /// <param name="xPoints">A list containing the x-coordinates.</param>
        /// <param name="yPoints">A list containing the y-coordinates.</param>
        /// <param name="xScale">The scale to be applied in the x-coordinates.</param>
        /// <param name="yScale">The scale to be applied in the y-coordinates.</param>
        /// <param name="thick">The thick (stroke) of the curve draw.</param>
        /// <param name="lineStyle">The line style of the curve draw.</param>
        /// <param name="markerType">The type of the marker for each dot.</param>
        /// <param name="markerSize">The size of the marker.</param>
        /// <param name="color">The color of the curve draw.</param>
        /// <param name="customColor">A boolean value indicating if the curve shall be drawn with a custom color.</param>
        /// <param name="RGBDescription">The RGB description (in the form of "R|G|B") of the color.</param>
        /// <param name="legend">A boolean value indicating if the curve name will apear in the legend box list.</param>
        /// <param name="secondY">A boolean value indicating if the curve needs to be refered to the second (right) Y-axis.</param>
        public DataSeriesObj(string name, List<double> xPoints, List<double> yPoints, double xScale, double yScale,
            double thick, OxyPlot.LineStyle lineStyle, OxyPlot.MarkerType markerType, double markerSize, SimplePlotterMisc.Enums.Colors color,
            bool customColor, string RGBDescription, bool legend, bool secondY)
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
            foreach (var item in Enum.GetValues(typeof(OxyPlot.MarkerType)))
            {
                availableMarkerTypes.Add((OxyPlot.MarkerType)item);
            }
            this.markerType = markerType;
            this.markerSize = markerSize;
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

        /// <summary>
        /// Gets or sets the name of the data series.</param>
        /// </summary>
        public string Name
        {
            get { return name; }
            set 
            {
                name = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a list containing the points of the curve (avoid to use the 'set').
        /// </summary>
        public List<PointObj> Points
        {
            get { return points; }
            set
            {
                points = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a list containing the x-coordinates.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a list containing the y-coordinates.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the scale to be applied in the x-coordinates.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the scale to be applied in the y-coordinates.
        /// </summary>
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

        /// <summary>
        /// Gets the number of points of the curve.
        /// </summary>
        public int Length
        {
            get { return points.Count; }
        }

        /// <summary>
        /// Gets or sets the thick (stroke) of the curve draw.
        /// </summary>
        public double Thick
        {
            get { return thick; }
            set
            {
                thick = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a list containing the available line styles.
        /// </summary>
        public List<OxyPlot.LineStyle> AvailableLineStyles
        {
            get { return availableLineStyles; }
            set
            {
                availableLineStyles = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the line style of the curve draw.
        /// </summary>
        public OxyPlot.LineStyle LineStyle
        {
            get { return lineStyle; }
            set
            {
                lineStyle = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a list containing the available marker types.
        /// </summary>
        public List<OxyPlot.MarkerType> AvailableMarkerTypes
        {
            get { return availableMarkerTypes; }
            set
            {
                availableMarkerTypes = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the marker type of the dots in the curve draw.
        /// </summary>
        public OxyPlot.MarkerType MarkerType
        {
            get { return markerType; }
            set
            {
                markerType = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the size of the markers.
        /// </summary>
        public double MarkerSize
        {
            get { return markerSize; }
            set
            {
                markerSize = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a list containing the available line colors.
        /// </summary>
        public List<Enums.Colors> AvailableColors
        {
            get { return availableColors; }
            set
            {
                availableColors = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the color of the curve draw.
        /// </summary>
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

        /// <summary>
        /// Gets the color (in the format suitable for OxyPlot).
        /// </summary>
        public OxyColor OxyColor
        {
            get { return OxyColor.FromRgb(rgb.Item1, rgb.Item2, rgb.Item3); }
        }

        /// <summary>
        /// Gets or sets a boolean value indicating if the curve shall be drawn with a custom color.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a boolean value indicating if the curve shall be drawn with a standard color.
        /// </summary>
        public bool StandardColor
        {
            get { return standardColor; }
            set
            {
                standardColor = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the RGB description (in the form of "R|G|B") of the color.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the RGB colors (best use RGBDescription).
        /// </summary>
        public Tuple<byte, byte, byte> RGB
        {
            get { return rgb; }
            set
            {
                rgb = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets whether the curve name needs to appear in the legend list or not.
        /// </summary>
        public bool Legend
        {
            get { return legend; }
            set
            {
                legend = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets whether the curve shall use the right (second) Y-axis or not.
        /// </summary>
        public bool SecondY
        {
            get { return secondY; }
            set
            {
                secondY = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a list of points of the curve to be used in a GIF.
        /// </summary>
        public List<PointObj> GIFPoints
        {
            get { return gifPoints; }
            set
            {
                gifPoints = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the key index which need to be plotted in each frame form the GIFPoints.
        /// </summary>
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

        /// <summary>
        /// Updates the points lists based on scale changes.
        /// </summary>
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

        /// <summary>
        /// Updates the points lists based on complete new parameters.
        /// </summary>
        /// <param name="scaleX">The new X-scale.</param>
        /// <param name="scaleY">The new Y-scale.</param>
        /// <param name="xPoints">The new x-coordinates.</param>
        /// <param name="yPoints">The new y-coordinates.</param>
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

        public static double GetDefaultDataSeriesThick() { return 1.5; }
        public static double GetDefaultDataSeriesMarkerSize() { return 3; }

        /// <summary>
        /// Generates the GIF points to be plotted on each frame.
        /// </summary>
        /// <param name="numberOfFrames">The desired number of frames.</param>
        /// <param name="interpolateData">A boolean value indicating if the data shall be interpolated or not (with few points, is better to interpolate, but it can be messy if the x-coordinates not only grow).</param>
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
