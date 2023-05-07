using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;
using System.Threading.Tasks;


namespace SimplePlotter
{
    internal class Program : Application
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application app = new Application();
            app.Run(new SimplePlotterView.MainWindow());
        }
    }
}
