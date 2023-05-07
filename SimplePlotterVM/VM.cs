using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
                    SimplePlotterMisc.DataSeriesController.Instance.AddDataSeries(name, SimplePlotterMisc.FileReader.GetFileData(item));
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
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<Point> selectedDataSeriesPoints = new ObservableCollection<Point>();
        public ObservableCollection<Point> SelectedDataSeriesPoints
        {
            get { return selectedDataSeriesPoints; }
            set
            {
                selectedDataSeriesPoints = value;
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

        #endregion

        #region PRIVATE METHODS

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
                foreach (var point in item.Points)
                {
                    serie.Points.Add(new OxyPlot.DataPoint(point.X, point.Y));
                }
                serie.Title = item.Name;
                plotObj.Series.Add(serie);
            }
            //refreshes to update axes
            plotObj.InvalidatePlot(true);
            //configures axes
            //x
            plotObj.Axes[0].Minimum = 0;
            plotObj.Axes[0].Maximum = 6;
            plotObj.Axes[0].Title = "Test - XAxis";
            //y
            plotObj.Axes[1].Minimum = 0;
            plotObj.Axes[1].Maximum = 20;
            //plotObj.Axes[1].MinorStep = 0.1;
            //plotObj.Axes[1].MajorStep = 0.5;
            plotObj.Axes[1].Title = "Test - YAxis";
            plotObj.Axes[1].AxisTitleDistance = 10;
            //finally refreshes
            plotObj.InvalidatePlot(true);
        }

    }
}
