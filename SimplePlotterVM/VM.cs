using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace SimplePlotterVM
{
    public class VM : Auxiliary.PropertyNotify
    {

        public VM() 
        {
            //commands
            Plot = new Auxiliary.DelegateCommand(plot);
            AddDataSeries = new Auxiliary.DelegateCommand(addDataSeries);
            RemoveDataSeries = new Auxiliary.DelegateCommand(removeDataSeries, canRemoveDataSeries);
            DataSeriesUp = new Auxiliary.DelegateCommand(dataSeriesUp, canMoveDataSeriesUp);
            DataSeriesDown = new Auxiliary.DelegateCommand(dataSeriesDown, canMoveDataSeriesDown);
            updateCompositeInfo();
            addSampleDataSeries();
        }

        #region COMMANDS

        public Auxiliary.DelegateCommand Plot { get; set; }
        public Auxiliary.DelegateCommand AddDataSeries { get; set; }
        public Auxiliary.DelegateCommand RemoveDataSeries { get; set; }
        public Auxiliary.DelegateCommand DataSeriesUp { get; set; }
        public Auxiliary.DelegateCommand DataSeriesDown { get; set; }

        #endregion

        #region COMMANDS ACTIONS

        private void plot(object parameter)
        {
            updateEntirePlot();
        }

        private void addDataSeries(object parameter)
        {
            Microsoft.Win32.OpenFileDialog myBrowser = new Microsoft.Win32.OpenFileDialog();
            myBrowser.Filter = "TextFiles|*.txt|CSV|*.csv|Data table|*.tab";
            myBrowser.DefaultExt = "txt";
            myBrowser.Multiselect = true;
            if (myBrowser.ShowDialog() == true)
            {
                foreach (var item in myBrowser.FileNames)
                {
                    string name = item.Split('\\').Last().Split('.')[0];
                    var points = SimplePlotterMisc.FileReader.GetFileData(item);
                    SimplePlotterMisc.DataSeriesController.Instance.AddDataSeries(name, points.Item1, points.Item2);
                }
                updateDataSeries();
            }
        }

        private void removeDataSeries(object parameter)
        {
            SimplePlotterMisc.DataSeriesController.Instance.RemoveDataSeries(selectedDataSeries);
            updateDataSeries();
        }

        private bool canRemoveDataSeries()
        {
            bool result = true;
            result &= selectedDataSeries != null;
            return result;
        }

        private void dataSeriesUp(object parameter)
        {
            SimplePlotterMisc.DataSeriesController.Instance.MoveDataSeriesUp(selectedDataSeries);
            updateDataSeries();
        }

        private bool canMoveDataSeriesUp()
        {
            bool result = true;
            result &= selectedDataSeries != null;
            result &= availableDataSeries.IndexOf(selectedDataSeries) != 0;
            return result;
        }

        private void dataSeriesDown(object parameter)
        {
            SimplePlotterMisc.DataSeriesController.Instance.MoveDataSeriesDown(selectedDataSeries);
            updateDataSeries();
        }

        private bool canMoveDataSeriesDown()
        {
            bool result = true;
            result &= selectedDataSeries != null;
            result &= availableDataSeries.IndexOf(selectedDataSeries) != availableDataSeries.Count - 1;
            return result;
        }

        #endregion

        #region PROPERTIES

        #region PLOT/CHART

        public OxyPlot.PlotModel plotObj = new OxyPlot.PlotModel();
        public OxyPlot.PlotModel PlotObj
        {
            get { return plotObj; }
            set
            {
                plotObj = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region DATA SERIES

        private ObservableCollection<SimplePlotterMisc.DataSeriesObj> availableDataSeries = new ObservableCollection<SimplePlotterMisc.DataSeriesObj>();
        public ObservableCollection<SimplePlotterMisc.DataSeriesObj> AvailableDataSeries
        {
            get { return availableDataSeries; }
            set
            {
                availableDataSeries = value;
                NotifyPropertyChanged();
            }
        }

        private SimplePlotterMisc.DataSeriesObj selectedDataSeries;
        public SimplePlotterMisc.DataSeriesObj SelectedDataSeries
        {
            get { return selectedDataSeries; }
            set
            {
                selectedDataSeries = value;
                if (selectedDataSeries != null) updateSelectedDataSeriesPoints();
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<SimplePlotterMisc.PointObj> selectedDataSeriesPoints = new ObservableCollection<SimplePlotterMisc.PointObj>();
        public ObservableCollection<SimplePlotterMisc.PointObj> SelectedDataSeriesPoints
        {
            get { return selectedDataSeriesPoints; }
            set
            {
                selectedDataSeriesPoints = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region AXIS

        private bool manualXMinAxisLimit;
        public bool ManualXMinAxisLimit
        {
            get { return manualXMinAxisLimit; }
            set
            {
                manualXMinAxisLimit = value;
                NotifyPropertyChanged();
            }
        }

        private bool manualXMaxAxisLimit;
        public bool ManualXMaxAxisLimit
        {
            get { return manualXMaxAxisLimit; }
            set
            {
                manualXMaxAxisLimit = value;
                NotifyPropertyChanged();
            }
        }

        private double xAxisMin;
        public double XAxisMin
        {
            get { return xAxisMin; }
            set
            {
                xAxisMin = value;
                NotifyPropertyChanged();
            }
        }

        private double xAxisMax;
        public double XAxisMax
        {
            get { return xAxisMax; }
            set
            {
                xAxisMax = value;
                NotifyPropertyChanged();
            }
        }

        private bool manualYMinAxisLimit;
        public bool ManualYMinAxisLimit
        {
            get { return manualYMinAxisLimit; }
            set
            {
                manualYMinAxisLimit = value;
                NotifyPropertyChanged();
            }
        }

        private bool manualYMaxAxisLimit;
        public bool ManualYMaxAxisLimit
        {
            get { return manualYMaxAxisLimit; }
            set
            {
                manualYMaxAxisLimit = value;
                NotifyPropertyChanged();
            }
        }

        private double yAxisMin;
        public double YAxisMin
        {
            get { return yAxisMin; }
            set
            {
                yAxisMin = value;
                NotifyPropertyChanged();
            }
        }

        private double yAxisMax;
        public double YAxisMax
        {
            get { return yAxisMax; }
            set
            {
                yAxisMax = value;
                NotifyPropertyChanged();
            }
        }

        private string xAxisTitle;
        public string XAxisTitle
        {
            get { return xAxisTitle; }
            set
            {
                xAxisTitle = value;
                updateTitles();
                NotifyPropertyChanged();
            }
        }

        private string yAxisTitle;
        public string YAxisTitle
        {
            get { return yAxisTitle; }
            set
            {
                yAxisTitle = value;
                updateTitles();
                NotifyPropertyChanged();
            }
        }

        private bool xLogarithmicScale;
        public bool XLogarithmicScale
        {
            get { return xLogarithmicScale; }
            set
            {
                xLogarithmicScale = value;
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private bool yLogarithmicScale;
        public bool YLogarithmicScale
        {
            get { return yLogarithmicScale; }
            set
            {
                yLogarithmicScale = value;
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region GRID LINES

        private bool xMajorGridLines;
        public bool XMajorGridLines
        {
            get { return xMajorGridLines; }
            set
            {
                xMajorGridLines = value;
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private bool yMajorGridLines;
        public bool YMajorGridLines
        {
            get { return yMajorGridLines; }
            set
            {
                yMajorGridLines = value;
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private bool xMinorGridLines;
        public bool XMinorGridLines
        {
            get { return xMinorGridLines; }
            set
            {
                xMinorGridLines = value;
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private bool yMinorGridLines;
        public bool YMinorGridLines
        {
            get { return yMinorGridLines; }
            set
            {
                yMinorGridLines = value;
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private double xMajorStep;
        public double XMajorStep
        {
            get { return xMajorStep; }
            set
            {
                xMajorStep = value;
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private double yMajorStep;
        public double YMajorStep
        {
            get { return yMajorStep; }
            set
            {
                yMajorStep = value;
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private double xMinorStep;
        public double XMinorStep
        {
            get { return xMinorStep; }
            set
            {
                xMinorStep = value;
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private double yMinorStep;
        public double YMinorStep
        {
            get { return yMinorStep; }
            set
            {
                yMinorStep = value;
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region CHART BOX

        private double chartWidth;
        public double ChartWidth
        {
            get { return chartWidth; }
            set
            {
                chartWidth = value;
                NotifyPropertyChanged();
            }
        }

        private double chartHeight;
        public double ChartHeight
        {
            get { return chartHeight; }
            set
            {
                chartHeight = value;
                NotifyPropertyChanged();
            }
        }

        private string chartTitle;
        public string ChartTitle
        {
            get { return chartTitle; }
            set
            {
                chartTitle = value;
                updateTitles();
                NotifyPropertyChanged();
            }
        }

        private bool showLegend;
        public bool ShowLegend
        {
            get { return showLegend; }
            set
            {
                showLegend = value;
                updateLegend();
                NotifyPropertyChanged();
            }
        }

        public List<OxyPlot.Legends.LegendPosition> availableLegendPositions = new List<LegendPosition>();
        public List<OxyPlot.Legends.LegendPosition> AvailableLegendPositions
        {
            get { return availableLegendPositions; }
            set
            {
                availableLegendPositions = value;
                updateLegend();
                NotifyPropertyChanged();
            }
        }

        public OxyPlot.Legends.LegendPosition selectedLegendPosition;
        public OxyPlot.Legends.LegendPosition SelectedLegendPosition
        {
            get { return selectedLegendPosition; }
            set
            {
                selectedLegendPosition = value;
                updateLegend();
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region FONTS

        private ObservableCollection<Enums.Fonts> availableFonts = new ObservableCollection<Enums.Fonts>();
        public ObservableCollection<Enums.Fonts> AvailableFonts
        {
            get { return availableFonts; }
            set
            {
                availableFonts = value;
                NotifyPropertyChanged();
            }
        }

        private Enums.Fonts selectedFont;
        public Enums.Fonts SelectedFont
        {
            get { return selectedFont; }
            set
            {
                selectedFont = value;
                updatePlotFonts();
                NotifyPropertyChanged();
            }
        }

        private double xAxisFontSize;
        public double XAxisFontSize
        {
            get { return xAxisFontSize; }
            set
            {
                xAxisFontSize = value;
                updatePlotFonts();
                NotifyPropertyChanged();
            }
        }

        private double yAxisFontSize;
        public double YAxisFontSize
        {
            get { return yAxisFontSize; }
            set
            {
                yAxisFontSize = value;
                updatePlotFonts();
                NotifyPropertyChanged();
            }
        }

        private double titleFontSize;
        public double TitleFontSize
        {
            get { return titleFontSize; }
            set
            {
                titleFontSize = value;
                updatePlotFonts();
                NotifyPropertyChanged();
            }
        }

        private double legendFontSize;
        public double LegendFontSize
        {
            get { return legendFontSize; }
            set
            {
                legendFontSize = value;
                updateLegend();
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region MISC

        public SPGlobalization.Vocabulary Vocabulary
        {
            get { return SPGlobalization.Vocabulary.Instance; }
        }

        private Exception ex;
        public Exception Ex
        {
            get { return ex; }
            set
            {
                ex = value;
                NotifyPropertyChanged();
            }
        }

        private MessageBoxResult answer;
        public MessageBoxResult Answer
        {
            get { return answer; }
            set
            {
                answer = value;
                NotifyPropertyChanged();
            }
        }

        private string question;
        public string Question
        {
            get { return question; }
            set
            {
                question = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region PRIVATE METHODS

        private void updateCompositeInfo()
        {
            manualXMinAxisLimit = false;
            manualXMaxAxisLimit = false;
            manualYMinAxisLimit = false;
            manualYMaxAxisLimit = false;
            xAxisTitle = "X";
            yAxisTitle = "Y";
            xLogarithmicScale = false;
            yLogarithmicScale = false;
            xMajorGridLines = false;
            yMajorGridLines = false;
            xMinorGridLines = false;
            yMinorGridLines = false;
            xMajorStep = 0;
            yMajorStep = 0;
            xMinorStep = 0;
            yMinorStep = 0;
            AvailableFonts.Clear();
            foreach (var item in Enum.GetValues(typeof(Enums.Fonts)))
            {
                AvailableFonts.Add((Enums.Fonts)item);
            }
            SelectedFont = Enums.Fonts.TimesNewRoman;
            xAxisFontSize = 0;
            yAxisFontSize = 0;
            titleFontSize = 0;
            legendFontSize = 0;
            chartWidth = 1000;
            chartHeight = 1000;
            chartTitle = "Sample title";
            foreach (var item in Enum.GetValues(typeof(OxyPlot.Legends.LegendPosition)))
            {
                AvailableLegendPositions.Add((OxyPlot.Legends.LegendPosition)item);
            }
            selectedLegendPosition = LegendPosition.TopRight;
        }

        private void updateDataSeries()
        {
            SimplePlotterMisc.DataSeriesObj sds = selectedDataSeries;
            availableDataSeries.Clear();
            foreach (var item in SimplePlotterMisc.DataSeriesController.Instance.DataSeries)
            {
                availableDataSeries.Add(item);
            }
            NotifyPropertyChanged("AvailableDataSeries");
            SelectedDataSeries = sds;
        }

        private void updateSelectedDataSeriesPoints()
        {
            selectedDataSeriesPoints.Clear();
            foreach (var item in selectedDataSeries.Points)
            {
                selectedDataSeriesPoints.Add(item);
            }
            NotifyPropertyChanged("SelectedDataSeriesPoints");
        }

        #endregion

        #region PLOT METHODS

        private void addSampleDataSeries()
        {
            SimplePlotterMisc.DataSeriesController.Instance.AddDataSeries("Example", new List<double> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new List<double> { 0, 1, 4, 8, 16, 32, 64, 128, 256, 512, 1024 });
            AvailableDataSeries.Add(SimplePlotterMisc.DataSeriesController.Instance.DataSeries[0]);
        }

        private void updateEntirePlot()
        {
            plotSeries();
            updateLegend();
            updateAxisScales();
            updateGridLines();
            updateTitles();
            updatePlotFonts();
        }

        public void plotSeries()
        {
            //cleares old series
            plotObj.Series.Clear();
            //adds new series
            foreach (var item in SimplePlotterMisc.DataSeriesController.Instance.DataSeries)
            {
                OxyPlot.Series.FunctionSeries serie = new OxyPlot.Series.FunctionSeries();
                for (int i = 0; i < item.Length; i++)
                {
                    serie.Points.Add(new OxyPlot.DataPoint(item.Points[i].ScaledX, item.Points[i].ScaledY));
                }
                serie.Title = item.Name;
                serie.StrokeThickness = item.Thick;
                serie.LineStyle = item.LineStyle;
                serie.Color = item.OxyColor;
                serie.RenderInLegend = item.Legend;
                plotObj.Series.Add(serie);
            }
            //refreshes to update axes
            plotObj.InvalidatePlot(true);
            //configures axes
            //x
            if (manualXMinAxisLimit) plotObj.Axes[0].Minimum = XAxisMin;
            if (manualXMinAxisLimit) plotObj.Axes[0].Maximum = XAxisMax;
            //y
            if (manualYMinAxisLimit) plotObj.Axes[1].Minimum = YAxisMin;
            if (manualYMinAxisLimit) plotObj.Axes[1].Maximum = YAxisMax;
            plotObj.Axes[1].AxisTitleDistance = 10;
            //finally refreshes
            plotObj.InvalidatePlot(true);
        }

        private void updateLegend()
        {
            PlotObj.Legends.Clear();
            if (showLegend)
            {
                string fontName = Auxiliary.Enumeration.GetEnumDescription(SelectedFont);
                plotObj.Legends.Add(new Legend
                {
                    LegendPlacement = LegendPlacement.Inside,
                    LegendPosition = selectedLegendPosition,
                    LegendOrientation = LegendOrientation.Vertical,
                    LegendFont = fontName,
                    LegendFontSize = legendFontSize,
                    LegendBackground = OxyColors.White,
                    LegendBorder = OxyColors.Black
                });
            }
            plotObj.InvalidatePlot(true);
        }

        private void updateTitles()
        {
            plotObj.Axes[0].Title = xAxisTitle;
            plotObj.Axes[1].Title = yAxisTitle;
            plotObj.Title = chartTitle;
            plotObj.InvalidatePlot(true);
        }

        private void updatePlotFonts()
        {
            string fontName = Auxiliary.Enumeration.GetEnumDescription(SelectedFont);
            plotObj.TitleFont = fontName;
            foreach (var item in plotObj.Axes)
            {
                item.Font = fontName;
                item.TitleFont = fontName;
            }
            if (plotObj.Axes.Count > 0)
            {
                plotObj.Axes[0].FontSize = xAxisFontSize == 0 ? double.NaN : xAxisFontSize;
                plotObj.Axes[1].FontSize = yAxisFontSize == 0 ? double.NaN : yAxisFontSize;
            }
            plotObj.TitleFontSize = titleFontSize == 0 ? double.NaN : titleFontSize;
            plotObj.InvalidatePlot(true);
        }

        private void updateAxisScales()
        {
            if (xLogarithmicScale)
            {
                plotObj.Axes[0] = new LogarithmicAxis { Position = AxisPosition.Bottom };
                plotObj.Axes[0].LabelFormatter = SimplePlotterMisc.LabelFormatters.Eng;
            }
            else
            {
                plotObj.Axes[0] = new LinearAxis { Position = AxisPosition.Bottom };
                plotObj.Axes[0].LabelFormatter = SimplePlotterMisc.LabelFormatters.Eng;
            }
            if (yLogarithmicScale)
            {
                plotObj.Axes[1] = new LogarithmicAxis { Position = AxisPosition.Left };
            }
            else
            {
                plotObj.Axes[1] = new LinearAxis { Position = AxisPosition.Left };
            }
            plotObj.InvalidatePlot(true);
        }

        private void updateGridLines()
        {
            //major
            if (XMajorGridLines)
            {
                plotObj.Axes[0].MajorGridlineStyle = LineStyle.Solid;
            }
            else
            {
                plotObj.Axes[0].MajorGridlineStyle = LineStyle.None;
            }
            if (YMajorGridLines)
            {
                plotObj.Axes[1].MajorGridlineStyle = LineStyle.Solid;
            }
            else
            {
                plotObj.Axes[1].MajorGridlineStyle = LineStyle.None;
            }
            plotObj.Axes[0].MajorStep = XMajorStep == 0 ? double.NaN : XMajorStep;
            plotObj.Axes[1].MajorStep = YMajorStep == 0 ? double.NaN : YMajorStep;
            //minor
            if (XMinorGridLines)
            {
                plotObj.Axes[0].MinorGridlineStyle = LineStyle.Dash;
            }
            else
            {
                plotObj.Axes[0].MinorGridlineStyle = LineStyle.None;
            }
            if (YMinorGridLines)
            {
                plotObj.Axes[1].MinorGridlineStyle = LineStyle.Dash;
            }
            else
            {
                plotObj.Axes[1].MinorGridlineStyle = LineStyle.None;
            }
            plotObj.Axes[0].MinorStep = XMinorStep == 0 ? double.NaN : XMinorStep;
            plotObj.Axes[1].MinorStep = YMinorStep == 0 ? double.NaN : YMinorStep;
            plotObj.InvalidatePlot(true);
        }

        #endregion

    }
}
