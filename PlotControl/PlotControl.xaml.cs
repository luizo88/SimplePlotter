using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PlotControl
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class PlotControl : UserControl
    {
        public PlotControl()
        {
            InitializeComponent();
            PlotObj = new OxyPlot.PlotModel();
        }

        public OxyPlot.PlotModel plotObj = new OxyPlot.PlotModel();
        public OxyPlot.PlotModel PlotObj
        {
            get { return plotObj; }
            set
            {
                plotObj = value;
                //NotifyPropertyChanged();
            }
        }

        public void doSomething()
        {
            //title
            plotObj.Title = "Test - plotObj";
            //cleares old series
            plotObj.Series.Clear();
            //adds new series
            OxyPlot.Series.FunctionSeries serie = new OxyPlot.Series.FunctionSeries();
            serie.Points.Add(new OxyPlot.DataPoint(0, 0));
            serie.Points.Add(new OxyPlot.DataPoint(1, 2));
            serie.Points.Add(new OxyPlot.DataPoint(2, 4));
            serie.Points.Add(new OxyPlot.DataPoint(3, 8));
            serie.Points.Add(new OxyPlot.DataPoint(4, 6));
            serie.Points.Add(new OxyPlot.DataPoint(5, 10));
            serie.Title = "Test - serieTitle";
            plotObj.Series.Add(serie);
            //refreshes to update axes
            plotObj.InvalidatePlot(true);
            //configures axes
            //x
            plotObj.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = OxyPlot.Axes.AxisPosition.Bottom, ExtraGridlines = new double[] { 0 }, ExtraGridlineThickness = 1, ExtraGridlineColor = OxyColors.Blue, });
            plotObj.Axes[0].Minimum = 0;
            plotObj.Axes[0].Maximum = 6;
            plotObj.Axes[0].Title = "Test - XAxis";
            configureGridLines(plotObj.Axes[0]);
            //y
            plotObj.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = OxyPlot.Axes.AxisPosition.Left, ExtraGridlines = new double[] { 0 }, ExtraGridlineThickness = 1, ExtraGridlineColor = OxyColors.Red, });
            plotObj.Axes[1].Minimum = 0;
            plotObj.Axes[1].Maximum = 12;
            plotObj.Axes[1].MinorStep = 0.1;
            plotObj.Axes[1].MajorStep = 0.5;
            plotObj.Axes[1].Title = "Test - YAxis";
            plotObj.Axes[1].AxisTitleDistance = 10;
            configureGridLines(plotObj.Axes[1]);
            //finally refreshes
            plotObj.InvalidatePlot(true);
        }

        private void configureGridLines(OxyPlot.Axes.Axis axis)
        {
            axis.FontSize = 10;
            axis.MajorGridlineStyle = OxyPlot.LineStyle.Dash;
            axis.MajorGridlineColor = OxyPlot.OxyColor.FromRgb(200, 200, 200);
            axis.MinorGridlineStyle = OxyPlot.LineStyle.Dot;
            axis.MinorGridlineColor = OxyPlot.OxyColor.FromRgb(220, 220, 220);
        }

    }
}
