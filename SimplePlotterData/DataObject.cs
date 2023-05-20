using OxyPlot.Legends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePlotterData
{
    [Serializable]
    public class DataObject
    {
        public List<string> DataSeriesName { get; set; }
        public List<double> DataSeriesScaleX { get; set; }
        public List<double> DataSeriesScaleY { get; set; }
        public List<double> DataSeriesThick { get; set; }
        public List<OxyPlot.LineStyle> DataSeriesLineStyle { get; set; }
        public List<SimplePlotterMisc.Enums.Colors> DataSeriesColor { get; set; }
        public List<bool> DataSeriesCustomColor { get; set; }
        public List<string> DataSeriesRGBDescription { get; set; }
        public List<bool> DataSeriesLegend { get; set; }
        public List<bool> DataSeriesSecondY { get; set; }
        public List<List<double>> DataSeriesXPoints { get; set; }
        public List<List<double>> DataSeriesYPoints { get; set; }
        public bool ManualXMinAxisLimit { get; set; }
        public bool ManualXMaxAxisLimit { get; set; }
        public double XAxisMin { get; set; }
        public double XAxisMax { get; set; }
        public bool ManualYMinAxisLimit { get; set; }
        public bool ManualYMaxAxisLimit { get; set; }
        public double YAxisMin { get; set; }
        public double YAxisMax { get; set; }
        public bool ManualY2MinAxisLimit { get; set; }
        public bool ManualY2MaxAxisLimit { get; set; }
        public double Y2AxisMin { get; set; }
        public double Y2AxisMax { get; set; }
        public string XAxisTitle { get; set; }
        public string YAxisTitle { get; set; }
        public string Y2AxisTitle { get; set; }
        public bool XLogarithmicScale { get; set; }
        public bool YLogarithmicScale { get; set; }
        public bool Y2LogarithmicScale { get; set; }
        public SimplePlotterMisc.Enums.AxisLabelFormats SelectedXAxisLabelFormat { get; set; }
        public SimplePlotterMisc.Enums.AxisLabelFormats SelectedYAxisLabelFormat { get; set; }
        public SimplePlotterMisc.Enums.AxisLabelFormats SelectedY2AxisLabelFormat { get; set; }
        public bool XMajorGridLines { get; set; }
        public bool YMajorGridLines { get; set; }
        public bool Y2MajorGridLines { get; set; }
        public bool XMinorGridLines { get; set; }
        public bool YMinorGridLines { get; set; }
        public bool Y2MinorGridLines { get; set; }
        public double XMajorStep { get; set; }
        public double YMajorStep { get; set; }
        public double Y2MajorStep { get; set; }
        public double XMinorStep { get; set; }
        public double YMinorStep { get; set; }
        public double Y2MinorStep { get; set; }
        public int ChartWidth { get; set; }
        public int ChartHeight { get; set; }
        public string ChartTitle { get; set; }
        public bool ShowLegend { get; set; }
        public OxyPlot.Legends.LegendPosition SelectedLegendPosition { get; set; }
        public bool ShowLegendArrows { get; set; }
        public SimplePlotterMisc.Enums.Fonts SelectedFont { get; set; }
        public double XAxisFontSize { get; set; }
        public double YAxisFontSize { get; set; }
        public double Y2AxisFontSize { get; set; }
        public double TitleFontSize { get; set; }
        public double LegendFontSize { get; set; }
        public SimplePlotterMisc.Enums.Colors SelectedBackColor { get; set; }
        public bool CustomBackColor { get; set; }
        public string BackColorRGBDescription { get; set; }
        public SimplePlotterMisc.Enums.Colors SelectedBackgroundColor { get; set; }
        public bool CustomBackgroundColor { get; set; }
        public string BackgroundColorRGBDescription { get; set; }
        public SimplePlotterMisc.Enums.Colors SelectedGridLinesColor { get; set; }
        public bool CustomGridLinesColor { get; set; }
        public string GridLinesColorRGBDescription { get; set; }
        public double GIFTotalTime { get; set; }
        public int GIFFramesPerSecond { get; set; }
        public bool InterpolateData { get; set; }

        public DataObject(List<SimplePlotterMisc.DataSeriesObj> dataSeries, bool manualXMinAxisLimit, bool manualXMaxAxisLimit,
            double xAxisMin, double xAxisMax, bool manualYMinAxisLimit, bool manualYMaxAxisLimit, double yAxisMin, double yAxisMax,
            bool manualY2MinAxisLimit, bool manualY2MaxAxisLimit, double y2AxisMin, double y2AxisMax,
            string xAxisTitle, string yAxisTitle, string y2AxisTitle, bool xLogarithmicScale, bool yLogarithmicScale, bool y2LogarithmicScale,
            SimplePlotterMisc.Enums.AxisLabelFormats selectedXAxisLabelFormat, SimplePlotterMisc.Enums.AxisLabelFormats selectedYAxisLabelFormat,
            SimplePlotterMisc.Enums.AxisLabelFormats selectedY2AxisLabelFormat,
            bool xMajorGridLines, bool yMajorGridLines, bool y2MajorGridLines, bool xMinorGridLines, bool yMinorGridLines, bool y2MinorGridLines,
            double xMajorStep, double yMajorStep, double y2MajorStep, double xMinorStep, double yMinorStep, double y2MinorStep, int chartWidth, int chartHeight, string chartTitle,
            bool showLegend, OxyPlot.Legends.LegendPosition selectedLegendPosition, bool showLegendArrows, SimplePlotterMisc.Enums.Fonts selectedFont,
            double xAxisFontSize, double yAxisFontSize, double y2AxisFontSize, double titleFontSize, double legendFontSize,
            SimplePlotterMisc.Enums.Colors selectedBackColor, bool customBackColor, string backColorRGBDescription,
            SimplePlotterMisc.Enums.Colors selectedBackgroundColor, bool customBackgroundColor, string backgroundColorRGBDescription,
            SimplePlotterMisc.Enums.Colors selectedGridLinesColor, bool customGridLinesColor, string gridLinesColorRGBDescription,
            double gifTotalTime, int gifFramesPerSecond, bool interpolateData)
        {
            //DataSeries
            DataSeriesName = new List<string>();
            DataSeriesScaleX = new List<double>();
            DataSeriesScaleY = new List<double>();
            DataSeriesThick = new List<double>();
            DataSeriesLineStyle = new List<OxyPlot.LineStyle>();
            DataSeriesColor = new List<SimplePlotterMisc.Enums.Colors>();
            DataSeriesCustomColor = new List<bool>();
            DataSeriesRGBDescription = new List<string>();
            DataSeriesLegend = new List<bool>();
            DataSeriesSecondY = new List<bool>();
            DataSeriesXPoints = new List<List<double>>();
            DataSeriesYPoints = new List<List<double>>();
            foreach (var item in dataSeries)
            {
                DataSeriesName.Add(item.Name);
                DataSeriesScaleX.Add(item.XScale);
                DataSeriesScaleY.Add(item.YScale);
                DataSeriesThick.Add(item.Thick);
                DataSeriesLineStyle.Add(item.LineStyle);
                DataSeriesColor.Add(item.Color);
                DataSeriesCustomColor.Add(item.CustomColor);
                DataSeriesRGBDescription.Add(item.RGBDescription);
                DataSeriesLegend.Add(item.Legend);
                DataSeriesSecondY.Add(item.SecondY);
                DataSeriesXPoints.Add(item.XPoints);
                DataSeriesYPoints.Add(item.YPoints);
            }
            //values
            ManualXMinAxisLimit = manualXMinAxisLimit;
            ManualXMaxAxisLimit = manualXMaxAxisLimit;
            XAxisMin = xAxisMin;
            XAxisMax = xAxisMax;
            ManualYMinAxisLimit = manualYMinAxisLimit;
            ManualYMaxAxisLimit = manualYMaxAxisLimit;
            YAxisMin = yAxisMin;
            YAxisMax = yAxisMax;
            ManualY2MinAxisLimit = manualY2MinAxisLimit;
            ManualY2MaxAxisLimit = manualY2MaxAxisLimit;
            Y2AxisMin = y2AxisMin;
            Y2AxisMax = y2AxisMax;
            XAxisTitle = xAxisTitle;
            YAxisTitle = yAxisTitle;
            Y2AxisTitle = y2AxisTitle;
            XLogarithmicScale = xLogarithmicScale;
            YLogarithmicScale = yLogarithmicScale;
            Y2LogarithmicScale = y2LogarithmicScale;
            SelectedXAxisLabelFormat = selectedXAxisLabelFormat;
            SelectedYAxisLabelFormat = selectedYAxisLabelFormat;
            SelectedY2AxisLabelFormat = selectedY2AxisLabelFormat;
            XMajorGridLines = xMajorGridLines;
            YMajorGridLines = yMajorGridLines;
            Y2MajorGridLines = y2MajorGridLines;
            XMinorGridLines = xMinorGridLines;
            YMinorGridLines = yMinorGridLines;
            Y2MinorGridLines = y2MinorGridLines;
            XMajorStep = xMajorStep;
            YMajorStep = yMajorStep;
            Y2MajorStep = y2MajorStep;
            XMinorStep = xMinorStep;
            YMinorStep = yMinorStep;
            Y2MinorStep = y2MinorStep;
            ChartWidth = chartWidth;
            ChartHeight = chartHeight;
            ChartTitle = chartTitle;
            ShowLegend = showLegend;
            SelectedLegendPosition = selectedLegendPosition;
            ShowLegendArrows = showLegendArrows;
            SelectedFont = selectedFont;
            XAxisFontSize = xAxisFontSize;
            YAxisFontSize = yAxisFontSize;
            Y2AxisFontSize = y2AxisFontSize;
            TitleFontSize = titleFontSize;
            LegendFontSize = legendFontSize;
            SelectedBackColor = selectedBackColor;
            CustomBackColor = customBackColor;
            BackColorRGBDescription = backColorRGBDescription;
            SelectedBackgroundColor = selectedBackgroundColor;
            CustomBackgroundColor = customBackgroundColor;
            BackgroundColorRGBDescription = backgroundColorRGBDescription;
            SelectedGridLinesColor = selectedGridLinesColor;
            CustomGridLinesColor = customGridLinesColor;
            GridLinesColorRGBDescription = gridLinesColorRGBDescription;
            GIFTotalTime = gifTotalTime;
            GIFFramesPerSecond = gifFramesPerSecond;
            InterpolateData = interpolateData;
        }

        public DataObject()
            : this(new List<SimplePlotterMisc.DataSeriesObj>(), false, false, 0, 0, false, false, 0, 0, false, false, 0, 0, "", "", "",
                  false, false, false, SimplePlotterMisc.Enums.AxisLabelFormats.Default, SimplePlotterMisc.Enums.AxisLabelFormats.Default,
                  SimplePlotterMisc.Enums.AxisLabelFormats.Default, false, false, false, false, false, false, 0, 0, 0, 0, 0, 0, 0, 0, "", false, LegendPosition.TopRight, false,
                  SimplePlotterMisc.Enums.Fonts.TimesNewRoman, 0, 0, 0, 0, 0, SimplePlotterMisc.Enums.Colors.White, false, "255|255|255",
                  SimplePlotterMisc.Enums.Colors.White, false, "255|255|255", SimplePlotterMisc.Enums.Colors.White, false, "255|255|255", 0, 0, false)
        { }

    }
}
