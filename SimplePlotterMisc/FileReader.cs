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
        public static Tuple<List<double>, List<double>> GetFileData(string pathfile)
        {
            Tuple<List<double>, List<double>> result = new Tuple<List<double>, List<double>>(new List<double>(), new List<double>());
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
                    double x = double.Parse(line[0].Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                    double y = double.Parse(line[1].Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                    result.Item1.Add(x);
                    result.Item2.Add(y);
                }
                //add remaining lines
                while (reader.Peek() >= 0)
                {
                    string[] line = reader.ReadLine().Split(separator.ToCharArray());
                    double x = double.Parse(line[0].Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                    double y = double.Parse(line[1].Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                    result.Item1.Add(x);
                    result.Item2.Add(y);
                }
            }
            return result;
        }

        public static List<char> digitList = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', 'e', '+', '-' };

    }
}
