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
        ///// <summary>
        ///// Returns a list of pair-coordinates from text-like files. It detects the separator.
        ///// </summary>
        ///// <param name="pathfile">The full path of the file.</param>
        ///// <returns></returns>
        //public static List<Tuple<List<double>, List<double>, string>> GetFileData(string pathfile)
        //{
        //    List<Tuple<List<double>, List<double>, string>> result = new List<Tuple<List<double>, List<double>, string>>();
        //    using (StreamReader reader = new StreamReader(pathfile))
        //    {
        //        //pending: add multicolumn files
        //        string separator = "";
        //        string firstLine = "";
        //        bool onFirstNumber = true;
        //        bool onSecondNumber = false;
        //        bool hasHeader = false;
        //        string headerLine = "";
        //        List<string> headers = new List<string>();
        //        //finds the separator
        //        if (reader.Peek() >= 0)
        //        {
        //            firstLine = reader.ReadLine();
        //            //check if it's a header
        //            if (!digitList.Contains(firstLine[0]))
        //            {
        //                hasHeader = true;
        //                headerLine = firstLine;
        //                firstLine = reader.ReadLine();
        //            }
        //            foreach (var item in firstLine)
        //            {
        //                if (!onSecondNumber)
        //                {
        //                    if (onFirstNumber)
        //                    {
        //                        if (!digitList.Contains(item))
        //                        {
        //                            onFirstNumber = false;
        //                            separator += item;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (!digitList.Contains(item)) separator += item;
        //                        else
        //                        {
        //                            onSecondNumber = true;
        //                        }
        //                    }
        //                }
        //            }
        //            string[] line = firstLine.Split(separator.ToCharArray());
        //            //read the headers
        //            if (hasHeader)
        //            {
        //                string[] head = headerLine.Split(separator.ToCharArray());
        //                for (int i = 0; i < head.Count(); i++)
        //                {
        //                    headers.Add(head[i]);
        //                }
        //            }
        //            else
        //            {
        //                int nSeries = line.Count() - 1;
        //                for (int i = 0; i < nSeries + 1; i++)
        //                {
        //                    headers.Add("");
        //                }
        //            }
        //            //adds the first line
        //            for (int i = 1; i < line.Count(); i++)
        //            {
        //                Tuple<List<double>, List<double>, string> serie = new Tuple<List<double>, List<double>, string>(new List<double>(), new List<double>(), headers[i]);
        //                double x = double.Parse(line[0].Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
        //                double y = double.Parse(line[i].Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
        //                serie.Item1.Add(x);
        //                serie.Item2.Add(y);
        //                result.Add(serie);
        //            }
        //        }
        //        //add remaining lines
        //        while (reader.Peek() >= 0)
        //        {
        //            string[] line = reader.ReadLine().Split(separator.ToCharArray());
        //            for (int i = 1; i < line.Count(); i++)
        //            {
        //                double x = double.Parse(line[0].Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
        //                double y = double.Parse(line[i].Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
        //                result[i - 1].Item1.Add(x);
        //                result[i - 1].Item2.Add(y);
        //            }
        //        }
        //    }
        //    return result;
        //}

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
                bool endIsSeparator = false;
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
                    if (firstLine.ToCharArray().Last().ToString() == separator)
                    {
                        endIsSeparator = true;
                    }
                    string[] line = firstLine.Split(separator.ToCharArray());
                    //read the headers
                    if (hasHeader)
                    {
                        string[] head = headerLine.Split(separator.ToCharArray());
                        for (int i = 0; i < head.Count() - (endIsSeparator ? 1 : 0); i++)
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
                    for (int i = 1; i < line.Count() - (endIsSeparator ? 1 : 0); i++)
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
                    for (int i = 1; i < line.Count() - (endIsSeparator ? 1 : 0); i++)
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
                string type = atpData.Item1.Rows[i][0].ToString();
                string n1 = atpData.Item1.Rows[i][1].ToString();
                string n2 = atpData.Item1.Rows[i][2].ToString();
                string seriesName = string.Format("{0}: {1}-{2}", type, n1, n2);
                Tuple<List<double>, List<double>, string> serie = new Tuple<List<double>, List<double>, string>(new List<double>(), new List<double>(), seriesName);
                for (int j = 0; j < atpData.Item3.steps; j++)
                {
                    double x = atpData.Item2[j, 0];
                    double y = atpData.Item2[j, i + 1];
                    serie.Item1.Add(x);
                    serie.Item2.Add(y);
                }
                result.Add(serie);
            }
            return result;
        }

        /// <summary>
        /// Returns a list of pair-coordinates from ltspice-like (polar format) files that are generate by LTSpice (the circuit simulator).
        /// </summary>
        /// <param name="pathfile">The full path of the file.</param>
        /// <param name="importPhase">A boolean value indicating if the phase shall be imported along with the magnitude.</param>
        /// <param name="limitMaxPhase">A boolean value indicating if the maximum phase shall be limited.</param>
        /// <param name="maxPhase">The maximum phase.</param>
        /// <param name="limitMinPhase">A boolean value indicating if the minimum phase shall be limited.</param>
        /// <param name="minPhase">The minimum phase.</param>
        /// <returns></returns>
        public static List<Tuple<List<double>, List<double>, string>> GetFileDataLTSpicePolar(string pathfile, bool importPhase, bool limitMaxPhase,
            double maxPhase, bool limitMinPhase, double minPhase)
        {
            List<Tuple<List<double>, List<double>, string>> result = new List<Tuple<List<double>, List<double>, string>>();
            System.IO.StreamReader file = new System.IO.StreamReader(pathfile);
            string line = file.ReadLine();
            //first line (headers)
            line = line.Replace("\t", " ").ToString();
            List<string> data = line.Split(' ').ToList();
            List<string> seriesNames = new List<string>();
            //ABS
            for (int i = 1; i < data.Count(); i++)
            {
                seriesNames.Add(data[i] + "_ABS");
                if (importPhase) seriesNames.Add(data[i] + "_ARG");
            }
            //data series
            List<List<double>> allData = new List<List<double>>();
            ///FREQ
            allData.Add(new List<double>());
            double seriesCountDevider = importPhase ? 2 : 1;
            for (int i = 0; i < seriesNames.Count / seriesCountDevider; i++)
            {
                //ABS
                allData.Add(new List<double>());
                //ARG
                if (importPhase) allData.Add(new List<double>());
            }
            while ((line = file.ReadLine()) != null)
            {
                line = line.Replace("\t", " ").ToString();
                data = line.Split(' ').ToList();
                //x
                allData[0].Add(Convert.ToDouble(data[0]));
                //y
                for (int i = 1; i <= seriesNames.Count / seriesCountDevider; i++)
                {
                    //ABS
                    string y = data[i].Split('d')[0].Substring(1);
                    allData[i].Add(Math.Pow(10, Convert.ToDouble(y) / 20));
                    //ARG
                    if (importPhase)
                    {
                        y = data[i].Split(',')[1];
                        y = y.Remove(y.Length - 2);
                        double yr = Convert.ToDouble(y);
                        if (limitMinPhase)
                        {
                            yr = yr < minPhase ? yr + 360 : yr;
                        }
                        if (limitMaxPhase)
                        {
                            yr = yr > maxPhase ? yr - 360 : yr;
                        }
                        allData[i + seriesNames.Count / 2].Add(yr);
                    }
                }
            }
            for (int i = 1; i <= seriesNames.Count; i++)
            {
                result.Add(new Tuple<List<double>, List<double>, string>(allData[0], allData[i], seriesNames[i - 1]));
            }
            return result;
        }

        public static List<char> digitList = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', 'e', 'E', '+', '-' };

    }

}
