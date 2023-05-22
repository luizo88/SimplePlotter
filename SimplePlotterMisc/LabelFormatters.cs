using OxyPlot.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePlotterMisc
{
    /// <summary>
    /// A class used to handle useful label for axis-coordinates.
    /// </summary>
    public static class LabelFormatters
    {
        /// <summary>
        /// Returns a function to be set in OxyPlot.
        /// </summary>
        /// <param name="style">The style to be used.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Func<double, string> GetLabelFormatter(Enums.AxisLabelFormats style)
        {
            switch (style)
            {
                case Enums.AxisLabelFormats.Default: return DoNothing;
                case Enums.AxisLabelFormats.Scientific: return Power10;
                case Enums.AxisLabelFormats.Engineering: return SI;
                default: throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Use the default OxyPlot labelling.
        /// </summary>
        /// <param name="input">The value to be formatted.</param>
        /// <returns></returns>
        public static string DoNothing(double input)
        {
            if (input > 100)
            {
                return Math.Round(input, 6).ToString();
            }
            else
            {
                return input.ToString();
            }
        }

        /// <summary>
        /// Applies the si suffix as labelling style.
        /// </summary>
        /// <param name="input">The value to be formatted.</param>
        /// <returns></returns>
        public static string SI(double input)
        {
            double res = double.NaN;
            string suffix = string.Empty;
            if (input > 999) input = Math.Round(input, 6);
            // Prevod malych hodnot
            if (Math.Abs(input) <= 0.001)
            {
                Dictionary<int, string> siLow = new Dictionary<int, string>
                {
                    [-12] = "p",
                    [-9] = "n",
                    [-6] = "μ",
                    [-3] = "m",
                    //[-2] = "c",
                    //[-1] = "d",
                };
                foreach (var v in siLow.Keys)
                {
                    if (input != 0 && Math.Abs(input) <= Math.Pow(10, v))
                    {
                        res = input * Math.Pow(10, Math.Abs(v));
                        suffix = siLow[v];
                        break;
                    }
                }
            }
            // Prevod velkych hodnot
            if (Math.Abs(input) >= 1000)
            {
                Dictionary<int, string> siHigh = new Dictionary<int, string>
                {
                    [12] = "T",
                    [9] = "G",
                    [6] = "M",
                    [3] = "k",
                    //[2] = "h",
                    //[1] = "da",
                };
                foreach (var v in siHigh.Keys)
                {
                    if (input != 0 && Math.Abs(input) >= Math.Pow(10, v))
                    {
                        res = input / Math.Pow(10, Math.Abs(v));
                        suffix = siHigh[v];
                        break;
                    }
                }
            }
            return double.IsNaN(res) ? Math.Round(input, 6).ToString() : $"{Math.Round(res, 6)}{suffix}";
        }

        /// <summary>
        /// Applies the log10 (something like 10^x) as labelling style.
        /// </summary>
        /// <param name="input">The value to be formatted.</param>
        /// <returns></returns>
        public static string Power10(this double input)
        {
            string format = "g4";
            if (input == 0) return "0";
            const string sup_signs = "⁺⁻⁼⁽⁾ⁿ";
            const string sup_digits = "⁰¹²³⁴⁵⁶⁷⁸⁹";
            if (double.IsNaN(input) || double.IsInfinity(input))
            {
                return input.ToString();
            }
            int num_sign = Math.Sign(input);
            input = Math.Abs(input);
            // group exponents in multiples of 3 (thousands)
            // otherwise use:
            int exp = (int)Math.Floor(Math.Round(Math.Log(input, 10), 6));
            // and handle the exp==1 case separetly to avoid 10¹
            input *= Math.Pow(10, -exp);
            int exp_sign = Math.Sign(exp);
            exp = Math.Abs(exp);
            // Build the exponent string 'dig' from right to left
            string dig = string.Empty;
            while (exp > 0)
            {
                int n = exp % 10;
                dig = sup_digits[n] + dig;
                exp = exp / 10;
            }
            // if has exponent and its negative prepend the superscript minus sign
            if (dig.Length > 0 && exp_sign < 0)
            {
                dig = sup_signs[1] + dig;
            }
            // prepend answer with minus if number is negative
            string sig = num_sign < 0 ? "-" : "";
            string result = string.Empty;
            if (dig.Length > 0)
            {
                // has exponent
                if (input == 1)
                {
                    result = $"{sig}10{dig}";
                }
                else
                {
                    result = $"{sig}{input.ToString(format)}×10{dig}";
                }
            }
            else
            {
                // no exponent
                result = $"{sig}{input.ToString(format)}";
            }
            return result;
        }

    }
}
