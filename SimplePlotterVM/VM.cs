using Auxiliary;
using ImageMagick;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using OxyPlot.Wpf;
using SimplePlotterMisc;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace SimplePlotterVM
{
    public class VM : Auxiliary.PropertyNotify
    {

        public VM() 
        {
            Version = "v. 1.3.0.7";
            //commands
            OpenFileCommand = new Auxiliary.DelegateCommand(openFile);
            SaveFileCommand = new Auxiliary.DelegateCommand(saveFile);
            InterfaceLanguageChangeCommand = new Auxiliary.DelegateCommand(changeInterfaceLanguage);
            RestoreInitialConfig = new Auxiliary.DelegateCommand(restoreInitialConfig);
            RefreshPlot = new Auxiliary.DelegateCommand(refreshPlot);
            CopyPlotToClipboard = new Auxiliary.DelegateCommand(copyPlotToClipboard);
            ExportPlot = new Auxiliary.DelegateCommand(exportPlot);
            AddDataSeries = new Auxiliary.DelegateCommand(addDataSeries);
            RemoveDataSeries = new Auxiliary.DelegateCommand(removeDataSeries, canRemoveDataSeries);
            DataSeriesUp = new Auxiliary.DelegateCommand(dataSeriesUp, canMoveDataSeriesUp);
            DataSeriesDown = new Auxiliary.DelegateCommand(dataSeriesDown, canMoveDataSeriesDown);
            ApplyColorTemplate = new Auxiliary.DelegateCommand(applyColorTemplate);
            CreateGIF = new Auxiliary.DelegateCommand(createGIF);
            updateCompositeInfo();
            addSampleDataSeries();
            //assings the event to update the graphic based on data series changes
            SimplePlotterMisc.DataSeriesController.Instance.PropertyChanged += onDataSeriesPropertyChanged;
            updateEntirePlot();
        }

        #region EVENTS

        void onDataSeriesPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SecondY") updateNeedOfSecondY();
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
                case "SecondY":
                    if (!longProcessRuning) updateEntirePlot();
                    break;
            }
        }

        #endregion

        #region COMMANDS

        public Auxiliary.DelegateCommand OpenFileCommand { get; set; }
        public Auxiliary.DelegateCommand SaveFileCommand { get; set; }
        public Auxiliary.DelegateCommand InterfaceLanguageChangeCommand { get; set; }
        public Auxiliary.DelegateCommand RestoreInitialConfig { get; set; }
        public Auxiliary.DelegateCommand RefreshPlot { get; }
        public Auxiliary.DelegateCommand CopyPlotToClipboard { get; }
        public Auxiliary.DelegateCommand ExportPlot { get; }
        public Auxiliary.DelegateCommand Plot { get; set; }
        public Auxiliary.DelegateCommand AddDataSeries { get; set; }
        public Auxiliary.DelegateCommand RemoveDataSeries { get; set; }
        public Auxiliary.DelegateCommand DataSeriesUp { get; set; }
        public Auxiliary.DelegateCommand DataSeriesDown { get; set; }
        public Auxiliary.DelegateCommand ApplyColorTemplate { get; set; }
        public Auxiliary.DelegateCommand CreateGIF { get; set; }

        #endregion

        #region COMMANDS ACTIONS

        private void openFile(object parameter)
        {
            LongProcessRuning = true;
            //gets the data
            SimplePlotterData.DataObject dtob = null;
            Microsoft.Win32.OpenFileDialog myBrowser = new Microsoft.Win32.OpenFileDialog();
            myBrowser.Filter = "XML files (*.txt)|*.xml";
            myBrowser.DefaultExt = "xml";
            if (myBrowser.ShowDialog() == true)
            {
                DataSeriesController.Instance.DataSeries.Clear();
                dtob = SimplePlotterData.FileManager.OpenXML(myBrowser.FileName);
                //gets data series
                for (int i = 0; i < dtob.DataSeriesName.Count; i++)
                {
                    DataSeriesController.Instance.DataSeries.Add(new DataSeriesObj(dtob.DataSeriesName[i], dtob.DataSeriesXPoints[i], dtob.DataSeriesYPoints[i], dtob.DataSeriesScaleX[i], dtob.DataSeriesScaleY[i],
                        dtob.DataSeriesThick[i], dtob.DataSeriesLineStyle[i], dtob.DataSeriesColor[i], dtob.DataSeriesCustomColor[i], dtob.DataSeriesRGBDescription[i], dtob.DataSeriesLegend[i], dtob.DataSeriesSecondY[i]));
                }
                //values
                ManualXMinAxisLimit = dtob.ManualXMinAxisLimit;
                ManualXMaxAxisLimit = dtob.ManualXMaxAxisLimit;
                XAxisMin = dtob.XAxisMin;
                XAxisMax = dtob.XAxisMax;
                ManualYMinAxisLimit = dtob.ManualYMinAxisLimit;
                ManualYMaxAxisLimit = dtob.ManualYMaxAxisLimit;
                YAxisMin = dtob.YAxisMin;
                YAxisMax = dtob.YAxisMax;
                ManualY2MinAxisLimit = dtob.ManualY2MinAxisLimit;
                ManualY2MaxAxisLimit = dtob.ManualY2MaxAxisLimit;
                Y2AxisMin = dtob.Y2AxisMin;
                Y2AxisMax = dtob.Y2AxisMax;
                XAxisTitle = dtob.XAxisTitle;
                YAxisTitle = dtob.YAxisTitle;
                Y2AxisTitle = dtob.Y2AxisTitle;
                XLogarithmicScale = dtob.XLogarithmicScale;
                YLogarithmicScale = dtob.YLogarithmicScale;
                Y2LogarithmicScale = dtob.Y2LogarithmicScale;
                SelectedXAxisLabelFormat = dtob.SelectedXAxisLabelFormat;
                SelectedYAxisLabelFormat = dtob.SelectedYAxisLabelFormat;
                SelectedY2AxisLabelFormat = dtob.SelectedY2AxisLabelFormat;
                XMajorGridLines = dtob.XMajorGridLines;
                YMajorGridLines = dtob.YMajorGridLines;
                Y2MajorGridLines = dtob.Y2MajorGridLines;
                XMinorGridLines = dtob.XMinorGridLines;
                YMinorGridLines = dtob.YMinorGridLines;
                Y2MinorGridLines = dtob.Y2MinorGridLines;
                XMajorStep = dtob.XMajorStep;
                YMajorStep = dtob.YMajorStep;
                Y2MajorStep = dtob.Y2MajorStep;
                XMinorStep = dtob.XMinorStep;
                YMinorStep = dtob.YMinorStep;
                Y2MinorStep = dtob.Y2MinorStep;
                ChartWidth = dtob.ChartWidth;
                ChartHeight = dtob.ChartHeight;
                ChartTitle = dtob.ChartTitle;
                ShowLegend = dtob.ShowLegend;
                SelectedLegendPosition = dtob.SelectedLegendPosition;
                ShowLegendArrows = dtob.ShowLegendArrows;
                SelectedFont = dtob.SelectedFont;
                XAxisFontSize = dtob.XAxisFontSize;
                YAxisFontSize = dtob.YAxisFontSize;
                Y2AxisFontSize = dtob.Y2AxisFontSize;
                TitleFontSize = dtob.TitleFontSize;
                LegendFontSize = dtob.LegendFontSize;
                SelectedBackColor = dtob.SelectedBackColor;
                CustomBackColor = dtob.CustomBackColor;
                BackColorRGBDescription = dtob.BackColorRGBDescription;
                SelectedBackgroundColor = dtob.SelectedBackgroundColor;
                CustomBackgroundColor = dtob.CustomBackgroundColor;
                BackgroundColorRGBDescription = dtob.BackgroundColorRGBDescription;
                SelectedGridLinesColor = dtob.SelectedGridLinesColor;
                CustomGridLinesColor = dtob.CustomGridLinesColor;
                GridLinesColorRGBDescription = dtob.GridLinesColorRGBDescription;
                GIFTotalTime = dtob.GIFTotalTime;
                GIFFramesPerSecond = dtob.GIFFramesPerSecond;
                InterpolateData = dtob.InterpolateData;
            }
            LongProcessRuning = false;
            updateNeedOfSecondY();
            updateDataSeries();
            updateSelectedDataSeriesPoints();
            updateEntirePlot();
        }

        private void saveFile(object parameter)
        {
            Microsoft.Win32.SaveFileDialog myBrowser = new Microsoft.Win32.SaveFileDialog();
            myBrowser.Filter = "XML files (*.txt)|*.xml";
            myBrowser.DefaultExt = "xml";
            myBrowser.FileName = "SimplePlotterFile.xml";
            if (myBrowser.ShowDialog() == true)
            {
                SimplePlotterData.DataObject dtOb = new SimplePlotterData.DataObject(AvailableDataSeries.ToList(), manualXMinAxisLimit, manualXMaxAxisLimit,
                    xAxisMin, xAxisMax, manualYMinAxisLimit, manualYMaxAxisLimit, yAxisMin, yAxisMax, manualY2MinAxisLimit, manualY2MaxAxisLimit, y2AxisMin, y2AxisMax,
                    xAxisTitle, yAxisTitle, y2AxisTitle, xLogarithmicScale, yLogarithmicScale, y2LogarithmicScale,
                    selectedXAxisLabelFormat, selectedYAxisLabelFormat, selectedY2AxisLabelFormat,
                    xMajorGridLines, yMajorGridLines, y2MajorGridLines, xMinorGridLines, yMinorGridLines, y2MinorGridLines,
                    xMajorStep, yMajorStep, y2MajorStep, xMinorStep, yMinorStep, y2MinorStep, chartWidth, chartHeight, chartTitle,
                    showLegend, selectedLegendPosition, showLegendArrows, selectedFont,
                    xAxisFontSize, yAxisFontSize, y2AxisFontSize, titleFontSize, legendFontSize,
                    selectedBackColor, customBackColor, backColorRGBDescription,
                    selectedBackgroundColor, customBackgroundColor, backgroundColorRGBDescription,
                    selectedGridLinesColor, customGridLinesColor, gridLinesColorRGBDescription,
                    gifTotalTime, gifFramesPerSecond, interpolateData);
                SimplePlotterData.FileManager.SaveXML(dtOb, myBrowser.FileName);
            }
        }

        private void changeInterfaceLanguage(object parameter)
        {
            SPGlobalization.Languages language = (SPGlobalization.Languages)Convert.ToInt32(parameter);
            SPGlobalization.Vocabulary.Instance.ActualLanguage = language;
            InterfaceLanguage = language;
        }

        private void restoreInitialConfig(object parameter)
        {
            setInitialConfig();
            updateEntirePlot();
        }

        private void refreshPlot(object parameter)
        {
            updateEntirePlot();
        }

        private void copyPlotToClipboard(object parameter)
        {
            //https://oxyplot.readthedocs.io/en/latest/export/export-png.html
            var pngExporter = new PngExporter { Width = ChartWidth, Height = ChartHeight };
            var bitmap = pngExporter.ExportToBitmap(plotObj);
            System.Windows.Clipboard.SetImage(bitmap);
        }

        private void exportPlot(object parameter)
        {
            //https://oxyplot.readthedocs.io/en/latest/export/export-png.html
            Microsoft.Win32.SaveFileDialog myBrowser = new Microsoft.Win32.SaveFileDialog();
            myBrowser.Filter = "PNG (*.png)|*.png";
            myBrowser.DefaultExt = "png";
            myBrowser.FileName = string.Format("Chart_{0}x{1}.png", ChartWidth, ChartHeight);
            if (myBrowser.ShowDialog() == true)
            {
                var pngExporter = new PngExporter { Width = ChartWidth, Height = ChartHeight };
                pngExporter.ExportToFile(PlotObj, myBrowser.FileName);
            }
        }

        private void addDataSeries(object parameter)
        {
            LongProcessRuning = true;
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
                updateSelectedDataSeriesPoints();
                updateEntirePlot();
            }
            LongProcessRuning = false;
        }

        private void removeDataSeries(object parameter)
        {
            int index = SimplePlotterMisc.DataSeriesController.Instance.DataSeries.IndexOf(selectedDataSeries);
            SimplePlotterMisc.DataSeriesController.Instance.RemoveDataSeries(selectedDataSeries);
            updateDataSeries();
            updateSelectedDataSeriesPoints();
            updateEntirePlot();
            if (SimplePlotterMisc.DataSeriesController.Instance.DataSeries.Count > 0) SelectedDataSeries = SimplePlotterMisc.DataSeriesController.Instance.DataSeries[Math.Min(index, SimplePlotterMisc.DataSeriesController.Instance.DataSeries.Count - 1)];
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

        private void applyColorTemplate(object parameter)
        {
            LongProcessRuning = true;
            var colorList = SimplePlotterMisc.ColorTemplateController.GetRGBListFromColorTemplate(selectedColorTemplate, availableDataSeries.Count);
            for (int i = 0; i < availableDataSeries.Count; i++)
            {
                availableDataSeries[i].CustomColor = true;
                availableDataSeries[i].RGBDescription = string.Format("{0}|{1}|{2}", colorList[i].Item1, colorList[i].Item2, colorList[i].Item3);
            }
            updateEntirePlot();
            LongProcessRuning = false;
        }

        private void createGIF(object parameter)
        {
            Microsoft.Win32.SaveFileDialog myBrowser = new Microsoft.Win32.SaveFileDialog();
            myBrowser.Filter = "GIF (*.gif)|*.gif";
            myBrowser.DefaultExt = "gif";
            myBrowser.FileName = string.Format("ChartGIF_{0}x{1}.gif", ChartWidth, ChartHeight);
            if (myBrowser.ShowDialog() == true)
            {
                List<BitmapSource> frames = new List<BitmapSource>();
                SimplePlotterMisc.DataSeriesController.Instance.GenerateGIFPointsForAllSeries(gifNumberOfPoints, interpolateData);
                for (int i = 0; i < gifNumberOfPoints; i++)
                {
                    updateEntirePlotToGIF(i + 1);
                    var pngExporter = new PngExporter { Width = ChartWidth, Height = ChartHeight };
                    frames.Add(pngExporter.ExportToBitmap(plotObj));
                }
                MagickImageCollection collection = GIFGen.GIFGen.GetGIFObject(frames, true, 100 / gifFramesPerSecond);
                collection.Write(myBrowser.FileName);
                collection.Dispose();
            }
        }

        #endregion

        #region PROPERTIES

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

        private ObservableCollection<SimplePlotterMisc.Enums.ColorTemplates> availableColorTemplates = new ObservableCollection<SimplePlotterMisc.Enums.ColorTemplates>();
        public ObservableCollection<SimplePlotterMisc.Enums.ColorTemplates> AvailableColorTemplates
        {
            get { return availableColorTemplates; }
            set
            {
                availableColorTemplates = value;
                NotifyPropertyChanged();
            }
        }

        private SimplePlotterMisc.Enums.ColorTemplates selectedColorTemplate;
        public SimplePlotterMisc.Enums.ColorTemplates SelectedColorTemplate
        {
            get { return selectedColorTemplate; }
            set
            {
                selectedColorTemplate = value;
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
                updateEntirePlot();
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
                updateEntirePlot();
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
                updateEntirePlot();
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
                updateEntirePlot();
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
                updateEntirePlot();
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
                updateEntirePlot();
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
                updateEntirePlot();
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
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private bool manualY2MinAxisLimit;
        public bool ManualY2MinAxisLimit
        {
            get { return manualY2MinAxisLimit; }
            set
            {
                manualY2MinAxisLimit = value;
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private bool manualY2MaxAxisLimit;
        public bool ManualY2MaxAxisLimit
        {
            get { return manualY2MaxAxisLimit; }
            set
            {
                manualY2MaxAxisLimit = value;
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private double y2AxisMin;
        public double Y2AxisMin
        {
            get { return y2AxisMin; }
            set
            {
                y2AxisMin = value;
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private double y2AxisMax;
        public double Y2AxisMax
        {
            get { return y2AxisMax; }
            set
            {
                y2AxisMax = value;
                updateEntirePlot();
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

        private string y2AxisTitle;
        public string Y2AxisTitle
        {
            get { return y2AxisTitle; }
            set
            {
                y2AxisTitle = value;
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

        private bool y2LogarithmicScale;
        public bool Y2LogarithmicScale
        {
            get { return y2LogarithmicScale; }
            set
            {
                y2LogarithmicScale = value;
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<SimplePlotterMisc.Enums.AxisLabelFormats> availableAxisLabelFormats = new ObservableCollection<SimplePlotterMisc.Enums.AxisLabelFormats>();
        public ObservableCollection<SimplePlotterMisc.Enums.AxisLabelFormats> AvailableAxisLabelFormats
        {
            get { return availableAxisLabelFormats; }
            set
            {
                availableAxisLabelFormats = value;
                NotifyPropertyChanged();
            }
        }

        private SimplePlotterMisc.Enums.AxisLabelFormats selectedXAxisLabelFormat;
        public SimplePlotterMisc.Enums.AxisLabelFormats SelectedXAxisLabelFormat
        {
            get { return selectedXAxisLabelFormat; }
            set
            {
                selectedXAxisLabelFormat = value;
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private SimplePlotterMisc.Enums.AxisLabelFormats selectedYAxisLabelFormat;
        public SimplePlotterMisc.Enums.AxisLabelFormats SelectedYAxisLabelFormat
        {
            get { return selectedYAxisLabelFormat; }
            set
            {
                selectedYAxisLabelFormat = value;
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private SimplePlotterMisc.Enums.AxisLabelFormats selectedY2AxisLabelFormat;
        public SimplePlotterMisc.Enums.AxisLabelFormats SelectedY2AxisLabelFormat
        {
            get { return selectedY2AxisLabelFormat; }
            set
            {
                selectedY2AxisLabelFormat = value;
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private bool hasSecondYAxis;
        public bool HasSecondYAxis
        {
            get { return hasSecondYAxis; }
            set
            {
                if (hasSecondYAxis == value) return;
                hasSecondYAxis = value;
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

        private bool y2MajorGridLines;
        public bool Y2MajorGridLines
        {
            get { return y2MajorGridLines; }
            set
            {
                y2MajorGridLines = value;
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

        private bool y2MinorGridLines;
        public bool Y2MinorGridLines
        {
            get { return y2MinorGridLines; }
            set
            {
                y2MinorGridLines = value;
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
                if (value >= 0)
                {
                    xMajorStep = value;
                    updateEntirePlot();
                    NotifyPropertyChanged();
                }
            }
        }

        private double yMajorStep;
        public double YMajorStep
        {
            get { return yMajorStep; }
            set
            {
                if (value >= 0)
                {
                    yMajorStep = value;
                    updateEntirePlot();
                    NotifyPropertyChanged();
                }
            }
        }

        private double y2MajorStep;
        public double Y2MajorStep
        {
            get { return y2MajorStep; }
            set
            {
                if (value >= 0)
                {
                    y2MajorStep = value;
                    updateEntirePlot();
                    NotifyPropertyChanged();
                }
            }
        }

        private double xMinorStep;
        public double XMinorStep
        {
            get { return xMinorStep; }
            set
            {
                if (value >= 0)
                {
                    xMinorStep = value;
                    updateEntirePlot();
                    NotifyPropertyChanged();
                }
            }
        }

        private double yMinorStep;
        public double YMinorStep
        {
            get { return yMinorStep; }
            set
            {
                if (value >= 0)
                {
                    yMinorStep = value;
                    updateEntirePlot();
                    NotifyPropertyChanged();
                }
            }
        }

        private double y2MinorStep;
        public double Y2MinorStep
        {
            get { return y2MinorStep; }
            set
            {
                if (value >= 0)
                {
                    y2MinorStep = value;
                    updateEntirePlot();
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #region CHART BOX

        private int chartWidth;
        public int ChartWidth
        {
            get { return chartWidth; }
            set
            {
                if (value > 0)
                {
                    chartWidth = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private int chartHeight;
        public int ChartHeight
        {
            get { return chartHeight; }
            set
            {
                if (value > 0)
                {
                    chartHeight = value;
                    NotifyPropertyChanged();
                }
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

        private ObservableCollection<OxyPlot.Legends.LegendPosition> availableLegendPositions = new ObservableCollection<LegendPosition>();
        public ObservableCollection<OxyPlot.Legends.LegendPosition> AvailableLegendPositions
        {
            get { return availableLegendPositions; }
            set
            {
                availableLegendPositions = value;
                updateLegend();
                NotifyPropertyChanged();
            }
        }

        private OxyPlot.Legends.LegendPosition selectedLegendPosition;
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

        private bool showLegendArrows;
        public bool ShowLegendArrows
        {
            get { return showLegendArrows; }
            set
            {
                showLegendArrows = value;
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region FONTS

        private ObservableCollection<SimplePlotterMisc.Enums.Fonts> availableFonts = new ObservableCollection<SimplePlotterMisc.Enums.Fonts>();
        public ObservableCollection<SimplePlotterMisc.Enums.Fonts> AvailableFonts
        {
            get { return availableFonts; }
            set
            {
                availableFonts = value;
                NotifyPropertyChanged();
            }
        }

        private SimplePlotterMisc.Enums.Fonts selectedFont;
        public SimplePlotterMisc.Enums.Fonts SelectedFont
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
                if (value > 0)
                {
                    xAxisFontSize = value;
                    updatePlotFonts();
                    NotifyPropertyChanged();
                }
            }
        }

        private double yAxisFontSize;
        public double YAxisFontSize
        {
            get { return yAxisFontSize; }
            set
            {
                if (value > 0)
                {
                    yAxisFontSize = value;
                    updatePlotFonts();
                    NotifyPropertyChanged();
                }
            }
        }

        private double y2AxisFontSize;
        public double Y2AxisFontSize
        {
            get { return y2AxisFontSize; }
            set
            {
                if (value > 0)
                {
                    y2AxisFontSize = value;
                    updatePlotFonts();
                    NotifyPropertyChanged();
                }
            }
        }

        private double titleFontSize;
        public double TitleFontSize
        {
            get { return titleFontSize; }
            set
            {
                if (value > 0)
                {
                    titleFontSize = value;
                    updatePlotFonts();
                    NotifyPropertyChanged();
                }
            }
        }

        private double legendFontSize;
        public double LegendFontSize
        {
            get { return legendFontSize; }
            set
            {
                if (value > 0)
                {
                    legendFontSize = value;
                    updateLegend();
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #region COLOR

        private ObservableCollection<SimplePlotterMisc.Enums.Colors> availableColors = new ObservableCollection<SimplePlotterMisc.Enums.Colors>();
        public ObservableCollection<SimplePlotterMisc.Enums.Colors> AvailableColors
        {
            get { return availableColors; }
            set
            {
                availableColors = value;
                NotifyPropertyChanged();
            }
        }

        private SimplePlotterMisc.Enums.Colors selectedBackColor;
        public SimplePlotterMisc.Enums.Colors SelectedBackColor
        {
            get { return selectedBackColor; }
            set
            {
                selectedBackColor = value;
                BackColorRGBDescription = SimplePlotterMisc.ColorTemplateController.GetRGBDescriptionFromColor(selectedBackColor);
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private bool customBackColor;
        public bool CustomBackColor
        {
            get { return customBackColor; }
            set
            {
                customBackColor = value;
                StandardBackColor = !customBackColor;
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private bool standardBackColor;
        public bool StandardBackColor
        {
            get { return standardBackColor; }
            set
            {
                standardBackColor = value;
                NotifyPropertyChanged();
            }
        }

        private string backColorRGBDescription;
        public string BackColorRGBDescription
        {
            get { return backColorRGBDescription; }
            set
            {
                if (SimplePlotterMisc.ColorTemplateController.ValidateRGBDescription(value))
                {
                    backColorRGBDescription = value;
                    updateEntirePlot();
                    NotifyPropertyChanged();
                }
            }
        }

        private SimplePlotterMisc.Enums.Colors selectedBackgroundColor;
        public SimplePlotterMisc.Enums.Colors SelectedBackgroundColor
        {
            get { return selectedBackgroundColor; }
            set
            {
                selectedBackgroundColor = value;
                BackgroundColorRGBDescription = SimplePlotterMisc.ColorTemplateController.GetRGBDescriptionFromColor(selectedBackgroundColor);
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private bool customBackgroundColor;
        public bool CustomBackgroundColor
        {
            get { return customBackgroundColor; }
            set
            {
                customBackgroundColor = value;
                StandardBackgroundColor = !customBackgroundColor;
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private bool standardBackgroundColor;
        public bool StandardBackgroundColor
        {
            get { return standardBackgroundColor; }
            set
            {
                standardBackgroundColor = value;
                NotifyPropertyChanged();
            }
        }

        private string backgroundColorRGBDescription;
        public string BackgroundColorRGBDescription
        {
            get { return backgroundColorRGBDescription; }
            set
            {
                if (SimplePlotterMisc.ColorTemplateController.ValidateRGBDescription(value))
                {
                    backgroundColorRGBDescription = value;
                    updateEntirePlot();
                    NotifyPropertyChanged();
                }
            }
        }

        private SimplePlotterMisc.Enums.Colors selectedGridLinesColor;
        public SimplePlotterMisc.Enums.Colors SelectedGridLinesColor
        {
            get { return selectedGridLinesColor; }
            set
            {
                selectedGridLinesColor = value;
                GridLinesColorRGBDescription = SimplePlotterMisc.ColorTemplateController.GetRGBDescriptionFromColor(selectedGridLinesColor);
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private bool customGridLinesColor;
        public bool CustomGridLinesColor
        {
            get { return customGridLinesColor; }
            set
            {
                customGridLinesColor = value;
                StandardGridLinesColor = !customGridLinesColor;
                updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private bool standardGridLinesColor;
        public bool StandardGridLinesColor
        {
            get { return standardGridLinesColor; }
            set
            {
                standardGridLinesColor = value;
                NotifyPropertyChanged();
            }
        }

        private string gridLinesColorRGBDescription;
        public string GridLinesColorRGBDescription
        {
            get { return gridLinesColorRGBDescription; }
            set
            {
                if (SimplePlotterMisc.ColorTemplateController.ValidateRGBDescription(value))
                {
                    gridLinesColorRGBDescription = value;
                    updateEntirePlot();
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #region MISC

        private bool longProcessRuning = false;
        public bool LongProcessRuning
        {
            get { return longProcessRuning; }
            set
            {
                if (value.Equals(longProcessRuning)) return;
                longProcessRuning = value;
                if (!longProcessRuning) updateEntirePlot();
                NotifyPropertyChanged();
            }
        }

        private string version;
        public string Version
        {
            get { return version; }
            set
            {
                version = value;
                NotifyPropertyChanged();
            }
        }

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

        private SPGlobalization.Languages interfaceLanguage;
        public SPGlobalization.Languages InterfaceLanguage
        {
            get { return interfaceLanguage; }
            set
            {
                interfaceLanguage = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region GIF

        private double gifTotalTime;
        public double GIFTotalTime
        {
            get { return gifTotalTime; }
            set
            {
                if (value > 0)
                {
                    gifTotalTime = value;
                    GIFNumberOfPoints = computeNumberOfFrames(gifTotalTime, gifFramesPerSecond);
                    NotifyPropertyChanged();
                }
            }
        }

        private int gifFramesPerSecond;
        public int GIFFramesPerSecond
        {
            get { return gifFramesPerSecond; }
            set
            {
                if (value > 0)
                {
                    gifFramesPerSecond = value;
                    GIFNumberOfPoints = computeNumberOfFrames(gifTotalTime, gifFramesPerSecond);
                    NotifyPropertyChanged();
                }
            }
        }

        private int gifNumberOfPoints;
        public int GIFNumberOfPoints
        {
            get { return gifNumberOfPoints; }
            set
            {
                gifNumberOfPoints = value;
                NotifyPropertyChanged();
            }
        }

        private bool interpolateData;
        public bool InterpolateData
        {
            get { return interpolateData; }
            set
            {
                interpolateData = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region PRIVATE METHODS

        private void updateCompositeInfo()
        {
            foreach (var item in Enum.GetValues(typeof(SimplePlotterMisc.Enums.ColorTemplates)))
            {
                AvailableColorTemplates.Add((SimplePlotterMisc.Enums.ColorTemplates)item);
            }
            
            foreach (var item in Enum.GetValues(typeof(SimplePlotterMisc.Enums.AxisLabelFormats)))
            {
                AvailableAxisLabelFormats.Add((SimplePlotterMisc.Enums.AxisLabelFormats)item);
            }
            AvailableFonts.Clear();
            foreach (var item in Enum.GetValues(typeof(SimplePlotterMisc.Enums.Fonts)))
            {
                AvailableFonts.Add((SimplePlotterMisc.Enums.Fonts)item);
            }
            foreach (var item in Enum.GetValues(typeof(OxyPlot.Legends.LegendPosition)))
            {
                AvailableLegendPositions.Add((OxyPlot.Legends.LegendPosition)item);
            }
            foreach (var item in Enum.GetValues(typeof(SimplePlotterMisc.Enums.Colors)))
            {
                AvailableColors.Add((SimplePlotterMisc.Enums.Colors)item);
            }
            setInitialConfig();
        }

        private void setInitialConfig()
        {
            manualXMinAxisLimit = false;
            manualXMaxAxisLimit = false;
            manualYMinAxisLimit = false;
            manualYMaxAxisLimit = false;
            manualY2MinAxisLimit = false;
            manualY2MaxAxisLimit = false;
            xAxisTitle = "X";
            yAxisTitle = "Y";
            y2AxisTitle = "Y2";
            xLogarithmicScale = false;
            yLogarithmicScale = false;
            y2LogarithmicScale = false;
            selectedXAxisLabelFormat = SimplePlotterMisc.Enums.AxisLabelFormats.Default;
            selectedYAxisLabelFormat = SimplePlotterMisc.Enums.AxisLabelFormats.Default;
            selectedY2AxisLabelFormat = SimplePlotterMisc.Enums.AxisLabelFormats.Default;
            xMajorGridLines = false;
            yMajorGridLines = false;
            y2MajorGridLines = false;
            xMinorGridLines = false;
            yMinorGridLines = false;
            y2MinorGridLines = false;
            xMajorStep = 0;
            yMajorStep = 0;
            y2MajorStep = 0;
            xMinorStep = 0;
            yMinorStep = 0;
            y2MinorStep = 0;
            SelectedFont = SimplePlotterMisc.Enums.Fonts.TimesNewRoman;
            xAxisFontSize = 0;
            yAxisFontSize = 0;
            y2AxisFontSize = 0;
            titleFontSize = 0;
            legendFontSize = 0;
            chartWidth = 500;
            chartHeight = 500;
            chartTitle = "Sample title";
            SelectedBackColor = SimplePlotterMisc.Enums.Colors.White;
            CustomBackColor = false;
            SelectedBackgroundColor = SimplePlotterMisc.Enums.Colors.White;
            CustomBackgroundColor = false;
            SelectedGridLinesColor = SimplePlotterMisc.Enums.Colors.Gray;
            CustomGridLinesColor = false;
            selectedLegendPosition = LegendPosition.TopRight;
            GIFTotalTime = 1;
            GIFFramesPerSecond = 10;
            InterpolateData = false;
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
            if (SimplePlotterMisc.DataSeriesController.Instance.DataSeries.Contains(sds)) SelectedDataSeries = sds;
        }

        private void updateSelectedDataSeriesPoints()
        {
            selectedDataSeriesPoints.Clear();
            if (selectedDataSeries != null)
            {
                foreach (var item in selectedDataSeries.Points)
                {
                    selectedDataSeriesPoints.Add(item);
                }
            }
            NotifyPropertyChanged("SelectedDataSeriesPoints");
        }

        private int computeNumberOfFrames(double totalGIFTime, int framesPerSecond)
        {
            return (int)Math.Round(totalGIFTime * framesPerSecond);
        }

        private void updateNeedOfSecondY()
        {
            var linq = from p
                           in SimplePlotterMisc.DataSeriesController.Instance.DataSeries
                       where p.SecondY
                       select p;
            HasSecondYAxis = linq.Count() > 0;
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
            if (!longProcessRuning)
            {
                updateLegend();
                updateAxis();
                updateGridLines();
                updateTitles();
                updatePlotFonts();
                updateColors();
                plotSeries(false);
                plotObj.InvalidatePlot(true);
            }
        }

        private void updateEntirePlotToGIF(int stepOfGIF)
        {
            if (!longProcessRuning)
            {
                updateLegend();
                updateAxis();
                updateGridLines();
                updateTitles();
                updatePlotFonts();
                updateColors();
                plotSeries(true, stepOfGIF);
                plotObj.InvalidatePlot(true);
            }
        }

        public void plotSeries(bool isForGIF, int stepOfGIF = 0)
        {
            //cleares old series
            plotObj.Series.Clear();
            //adds new series
            foreach (var item in SimplePlotterMisc.DataSeriesController.Instance.DataSeries)
            {
                OxyPlot.Series.FunctionSeries serie = new OxyPlot.Series.FunctionSeries();
                if (isForGIF)
                {
                    int limitIndexToPlot = item.GIFKeyIndexes[stepOfGIF - 1];
                    for (int i = 0; i <= limitIndexToPlot; i++)
                    {
                        serie.Points.Add(new OxyPlot.DataPoint(item.GIFPoints[i].ScaledX, item.GIFPoints[i].ScaledY));
                    }
                }
                else
                {
                    for (int i = 0; i < item.Length; i++)
                    {
                        serie.Points.Add(new OxyPlot.DataPoint(item.Points[i].ScaledX, item.Points[i].ScaledY));
                    }
                }
                serie.Title = string.Format("{0}{1}", (showLegendArrows ? (item.SecondY ? "[→] " : "[←] ") : ""), item.Name);
                serie.StrokeThickness = item.Thick;
                serie.LineStyle = item.LineStyle;
                serie.Color = item.OxyColor;
                serie.RenderInLegend = item.Legend;
                if (item.SecondY) serie.YAxisKey = "Y2";
                plotObj.Series.Add(serie);
            }
            //refreshes to update axes
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
            if (plotObj.Axes.Count > 0)
            {
                plotObj.Axes[0].Title = xAxisTitle;
                plotObj.Axes[1].Title = yAxisTitle;
                if (plotObj.Axes.Count > 2) plotObj.Axes[2].Title = y2AxisTitle;
                plotObj.Title = chartTitle;
                plotObj.InvalidatePlot(true);
            }
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
                if (plotObj.Axes.Count > 2) plotObj.Axes[2].FontSize = y2AxisFontSize == 0 ? double.NaN : y2AxisFontSize;
            }
            plotObj.TitleFontSize = titleFontSize == 0 ? double.NaN : titleFontSize;
            plotObj.InvalidatePlot(true);
        }

        private void updateAxis()
        {
            if (plotObj.Axes.Count > 0)
            {
                if (xLogarithmicScale)
                {
                    plotObj.Axes[0] = new LogarithmicAxis { Key = "X1", Position = AxisPosition.Bottom };
                }
                else
                {
                    plotObj.Axes[0] = new LinearAxis { Key = "X1", Position = AxisPosition.Bottom };
                }
                if (yLogarithmicScale)
                {
                    plotObj.Axes[1] = new LogarithmicAxis { Key = "Y1", Position = AxisPosition.Left };
                }
                else
                {
                    plotObj.Axes[1] = new LinearAxis { Key = "Y1", Position = AxisPosition.Left };
                }
                if (HasSecondYAxis)
                {
                    if (plotObj.Axes.Count < 3) plotObj.Axes.Add(new LinearAxis() { Key = "Y2", Position = AxisPosition.Right });
                    if (y2LogarithmicScale)
                    {
                        plotObj.Axes[2] = new LogarithmicAxis { Key = "Y2", Position = AxisPosition.Right };
                    }
                    else
                    {
                        plotObj.Axes[2] = new LinearAxis { Key = "Y2", Position = AxisPosition.Right };
                    }
                }
                else
                {
                    if (plotObj.Axes.Count > 2) plotObj.Axes.RemoveAt(2);
                }
                plotObj.Axes[0].LabelFormatter = SimplePlotterMisc.LabelFormatters.GetLabelFormatter(selectedXAxisLabelFormat);
                plotObj.Axes[1].LabelFormatter = SimplePlotterMisc.LabelFormatters.GetLabelFormatter(selectedYAxisLabelFormat);
                if (plotObj.Axes.Count > 2) plotObj.Axes[2].LabelFormatter = SimplePlotterMisc.LabelFormatters.GetLabelFormatter(selectedY2AxisLabelFormat);
                //limits
                //x
                if (manualXMinAxisLimit) plotObj.Axes[0].Minimum = XAxisMin;
                if (manualXMaxAxisLimit) plotObj.Axes[0].Maximum = XAxisMax;
                //y
                if (manualYMinAxisLimit) plotObj.Axes[1].Minimum = YAxisMin;
                if (manualYMaxAxisLimit) plotObj.Axes[1].Maximum = YAxisMax;
                //y2
                if (plotObj.Axes.Count > 2)
                {
                    if (manualY2MinAxisLimit) plotObj.Axes[2].Minimum = Y2AxisMin;
                    if (manualY2MaxAxisLimit) plotObj.Axes[2].Maximum = Y2AxisMax;
                }
                plotObj.InvalidatePlot(true);
            }
        }

        private void updateGridLines()
        {
            if (plotObj.Axes.Count > 0)
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
                if (plotObj.Axes.Count > 2)
                {
                    if (Y2MajorGridLines)
                    {
                        plotObj.Axes[2].MajorGridlineStyle = LineStyle.Solid;
                    }
                    else
                    {
                        plotObj.Axes[2].MajorGridlineStyle = LineStyle.None;
                    }
                }
                plotObj.Axes[0].MajorStep = XMajorStep == 0 ? double.NaN : XMajorStep;
                plotObj.Axes[1].MajorStep = YMajorStep == 0 ? double.NaN : YMajorStep;
                if (plotObj.Axes.Count > 2) plotObj.Axes[2].MajorStep = Y2MajorStep == 0 ? double.NaN : Y2MajorStep;
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
                if (plotObj.Axes.Count > 2)
                {
                    if (Y2MinorGridLines)
                    {
                        plotObj.Axes[2].MinorGridlineStyle = LineStyle.Dash;
                    }
                    else
                    {
                        plotObj.Axes[2].MinorGridlineStyle = LineStyle.None;
                    }
                }
                plotObj.Axes[0].MinorStep = XMinorStep == 0 ? double.NaN : XMinorStep;
                plotObj.Axes[1].MinorStep = YMinorStep == 0 ? double.NaN : YMinorStep;
                if (plotObj.Axes.Count > 2) plotObj.Axes[2].MinorStep = Y2MinorStep == 0 ? double.NaN : Y2MinorStep;
                plotObj.InvalidatePlot(true);
            }
        }

        private void updateColors()
        {
            if (backColorRGBDescription != null) PlotObj.Background = ColorTemplateController.GetOxyColorFromRGBDescription(backColorRGBDescription);
            if (backgroundColorRGBDescription != null) PlotObj.PlotAreaBackground = ColorTemplateController.GetOxyColorFromRGBDescription(backgroundColorRGBDescription);
            if (gridLinesColorRGBDescription != null)
            {
                if (PlotObj.Axes.Count > 0)
                {
                    plotObj.Axes[0].MajorGridlineColor = ColorTemplateController.GetOxyColorFromRGBDescription(gridLinesColorRGBDescription);
                    plotObj.Axes[1].MajorGridlineColor = ColorTemplateController.GetOxyColorFromRGBDescription(gridLinesColorRGBDescription);
                    plotObj.Axes[0].MinorGridlineColor = ColorTemplateController.GetOxyColorFromRGBDescription(gridLinesColorRGBDescription);
                    plotObj.Axes[1].MinorGridlineColor = ColorTemplateController.GetOxyColorFromRGBDescription(gridLinesColorRGBDescription);
                }
            }
            plotObj.InvalidatePlot(true);
        }

        #endregion

    }
}
