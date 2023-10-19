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
        public static List<Tuple<List<double>, List<double>, string>> GetFileData(string pathfile)
        {
            List<Tuple<List<double>, List<double>, string>> result = new List<Tuple<List<double>, List<double>, string>>();
            using (StreamReader reader = new StreamReader(pathfile))
            {
                //pending: add multicolumn files
                string separator = "";
                string firstLine = "";
                bool onFirstNumber = true;
                bool onSecondNumber = false;
                bool hasHeader = false;
                string headerLine = "";
                List<string> headers = new List<string>();
                //finds the separator
                if (reader.Peek() >= 0)
                {
                    firstLine = reader.ReadLine();
                    //check if it's a header
                    if (!digitList.Contains(firstLine[0]))
                    {
                        hasHeader = true;
                        headerLine = firstLine;
                        firstLine = reader.ReadLine();
                    }
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
                    string[] line = firstLine.Split(separator.ToCharArray());
                    //read the headers
                    if (hasHeader)
                    {
                        string[] head = headerLine.Split(separator.ToCharArray());
                        for (int i = 0; i < head.Count(); i++)
                        {
                            headers.Add(head[i]);
                        }
                    }
                    else
                    {
                        int nSeries = line.Count() - 1;
                        for (int i = 0; i < nSeries + 1; i++)
                        {
                            headers.Add("");
                        }
                    }
                    //adds the first line
                    for (int i = 1; i < line.Count(); i++)
                    {
                        Tuple<List<double>, List<double>, string> serie = new Tuple<List<double>, List<double>, string>(new List<double>(), new List<double>(), headers[i]);
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

        /// <summary>
        /// Returns a list of pair-coordinates from pl4-like files that are generate by ATP (the circuit simulator).
        /// </summary>
        /// <param name="pathfile">The full path of the file.</param>
        /// <returns></returns>
        public static List<Tuple<List<double>, List<double>, string>> GetFileDataFromPL4File(string pathfile)
        {
            List<Tuple<List<double>, List<double>, string>> result = new List<Tuple<List<double>, List<double>, string>>();
            var atpData = SimplePlotterMisc.PL4Reader.ReadPL4(pathfile);
            for (int i = 0; i < atpData.Item1.Rows.Count; i++)
            {
                string type = PL4Reader.ConvertType(Convert.ToInt32(atpData.Item1.Rows[i][0]));
                string n1 = atpData.Item1.Rows[i][1].ToString();
                string n2 = atpData.Item1.Rows[i][2].ToString();
                string seriesName = string.Format("{0}:{1}-{2}", type, n1, n2);
                Tuple<List<double>, List<double>, string> serie = new Tuple<List<double>, List<double>, string>(new List<double>(), new List<double>(), seriesName);
                for (int j = 0; j < atpData.Item3.steps; j++)
                {
                    double x = atpData.Item2[j, 0];
                    double y = atpData.Item2[j, i];
                    serie.Item1.Add(x);
                    serie.Item2.Add(y);
                }
                result.Add(serie);
            }
            return result;
        }

        public static List<char> digitList = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', 'e', '+', '-' };

    }

}
