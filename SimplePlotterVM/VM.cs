﻿using OxyPlot;
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
            DoSomething = new Auxiliary.DelegateCommand(doSomething);
            AddDataSeries = new Auxiliary.DelegateCommand(addDataSeries);
            RemoveDataSeries = new Auxiliary.DelegateCommand(removeDataSeries, canRemoveDataSeries);
            DataSeriesUp = new Auxiliary.DelegateCommand(dataSeriesUp, canMoveDataSeriesUp);
            DataSeriesDown = new Auxiliary.DelegateCommand(dataSeriesDown, canMoveDataSeriesDown);
            updateCompositeInfo();
            addSampleDataSeries();
        }

        #region COMMANDS

        public Auxiliary.DelegateCommand DoSomething { get; set; }
        public Auxiliary.DelegateCommand AddDataSeries { get; set; }
        public Auxiliary.DelegateCommand RemoveDataSeries { get; set; }
        public Auxiliary.DelegateCommand DataSeriesUp { get; set; }
        public Auxiliary.DelegateCommand DataSeriesDown { get; set; }

        #endregion

        #region COMMANDS ACTIONS

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
            AvailableFonts.Clear();
            foreach (var item in Enum.GetValues(typeof(Enums.Fonts)))
            {
                AvailableFonts.Add((Enums.Fonts)item);
            }
            SelectedFont = Enums.Fonts.TimesNewRoman;
            chartWidth = 1000;
            chartHeight = 1000;
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
            SimplePlotterMisc.DataSeriesController.Instance.AddDataSeries("Example", new List<double> { 0, 1, 2, 3, 4, 5, 6 }, new List<double> { 0, 1, 4, 8, 16, 32, 64 });
            AvailableDataSeries.Add(SimplePlotterMisc.DataSeriesController.Instance.DataSeries[0]);
        }

        private void updateEntirePlot()
        {
            updateTitles();
            updatePlotFonts();
        }

        private void updateTitles()
        {
            plotObj.Axes[0].Title = xAxisTitle;
            plotObj.Axes[1].Title = yAxisTitle;
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
            plotObj.InvalidatePlot(true);
        }

        #endregion

        public void doSomething(object parameter)
        {
            //title
            plotObj.Title = "Test - plotObj";
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
            //plotObj.Axes[1].MinorStep = 0.1;
            //plotObj.Axes[1].MajorStep = 0.5;
            plotObj.Axes[1].AxisTitleDistance = 10;
            //finally refreshes
            updateEntirePlot();
            plotObj.InvalidatePlot(true);
        }

    }
}
