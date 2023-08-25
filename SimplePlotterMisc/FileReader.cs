using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplePlotterMisc
{
    /// <summary>
    /// A class used to read and to interpret files.
    /// </summary>
    public class FileReader
    {
        /// <summary>
        /// Returns a list of pair-coordinates from text-like files. It detects the separator.
        /// </summary>
        /// <param name="pathfile">The full path of the file.</param>
        /// <returns></returns>
        public static List<Tuple<List<double>, List<double>>> GetFileData(string pathfile)
        {
            List<Tuple<List<double>, List<double>>> result = new List<Tuple<List<double>, List<double>>>();
            using (StreamReader reader = new StreamReader(pathfile))
            {
                //pending: add multicolumn files
                string separator = "";
                string firstLine = "";
                bool onFirstNumber = true;
                bool onSecondNumber = false;
                //finds the separator
                if (reader.Peek() >= 0)
                {
                    firstLine = reader.ReadLine();
                    foreach (var item in firstLine)
                    {
                        if (!onSecondNumber)
                        {
                            if (onFirstNumber)
                            {
                                if (!digitList.Contains(item))
                                {
                                    onFirstNumber = false;
                                    separator += item;
                                }
                            }
                            else
                            {
                                if (!digitList.Contains(item)) separator += item;
                                else
                                {
                                    onSecondNumber = true;
                                }
                            }
                        }
                    }
                    //adds the first line
                    string[] line = firstLine.Split(separator.ToCharArray());
                    for (int i = 1; i < line.Count(); i++)
                    {
                        Tuple<List<double>, List<double>> serie = new Tuple<List<double>, List<double>>(new List<double>(), new List<double>());
                        double x = double.Parse(line[0].Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                        double y = double.Parse(line[i].Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                        serie.Item1.Add(x);
                        serie.Item2.Add(y);
                        result.Add(serie);
                    }
                }
                //add remaining lines
                while (reader.Peek() >= 0)
                {
                    string[] line = reader.ReadLine().Split(separator.ToCharArray());
                    for (int i = 1; i < line.Count(); i++)
                    {
                        double x = double.Parse(line[0].Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                        double y = double.Parse(line[i].Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                        result[i - 1].Item1.Add(x);
                        result[i - 1].Item2.Add(y);
                    }
                    
                }
            }
            return result;
        }

        public static List<char> digitList = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', 'e', '+', '-' };

    }
}
