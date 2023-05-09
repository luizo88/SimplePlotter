using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePlotterMisc
{
    public static class LabelFormatters
    {
        public static string SI(double input)
        {
            double res = double.NaN;
            string suffix = string.Empty;
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

        public static string Eng(this double x)
        {
            string format = "g4";
            if (x == 0) return "0";
            const string sup_signs = "⁺⁻⁼⁽⁾ⁿ";
            const string sup_digits = "⁰¹²³⁴⁵⁶⁷⁸⁹";
            if (double.IsNaN(x) || double.IsInfinity(x))
            {
                return x.ToString();
            }
            int num_sign = Math.Sign(x);
            x = Math.Abs(x);
            // group exponents in multiples of 3 (thousands)
            //int exp = (int)Math.Floor(Math.Log(x, 10) / 3) * 3;
            // otherwise use:
            int exp = (int)Math.Floor(Math.Log(x, 10));
            // and handle the exp==1 case separetly to avoid 10¹
            x *= Math.Pow(10, -exp);
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
                result =  $"{sig}{x.ToString(format)}×10{dig}";
            }
            else
            {
                // no exponent
                result = $"{sig}{x.ToString(format)}";
            }
            //return $"{result,12}";
            return result;
        }

        public static String Power10(double input)
        {
            string SuperscriptDigits = "\u2070\u00b9\u00b2\u00b3\u2074\u2075\u2076\u2077\u2078\u2079";
            string expstr = String.Format("{0:0.#E0}", input);
            var numparts = expstr.Split('E');
            char[] powerchars = numparts[1].ToArray();
            for (int i = 0; i < powerchars.Length; i++)
            {
                powerchars[i] = (powerchars[i] == '-') ? '\u207b' : SuperscriptDigits[powerchars[i] - '0'];
            }
            numparts[1] = new String(powerchars);
            return String.Join(" x 10", numparts);
        }

    }
}
