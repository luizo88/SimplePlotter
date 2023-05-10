using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplePlotterMisc
{
    public class ColorTemplateController : Auxiliary.PropertyNotify
    {
        private static ColorTemplateController instance = new ColorTemplateController();
        List<DataSeriesObj> dataSeries = new List<DataSeriesObj>();

        public ColorTemplateController() 
        {

        }

        #region PRIVATE METHODS

        private static List<Tuple<byte, byte, byte>> getRainbowColors(int numberOfColors)
        {
            List<Tuple<byte, byte, byte>> result = new List<Tuple<byte, byte, byte>>();
            for (int i = 0; i < numberOfColors; i++)
            {
                double factor = i * 1.0 / numberOfColors;
                int red = Math.Max(Math.Min(255, (int)Math.Round(480.01 * factor + 14.955, 0)), 0);
                int green = Math.Max(Math.Min(255, (int)Math.Round(-480.01 * factor + 494.96, 0)), 0);
                int blue = Math.Max(Math.Min(255, (int)Math.Round(999.35 * Math.Pow(factor, 2) - 999.35 * factor + 246.39, 0)), 0);
                result.Add(new Tuple<byte, byte, byte>((byte)red, (byte)green, (byte)blue));
            }
            return result;
        }

        private static List<Tuple<byte, byte, byte>> getCycleColors(int numberOfColors)
        {
            List<Tuple<byte, byte, byte>> result = new List<Tuple<byte, byte, byte>>();
            List<Enums.Colors> l = new List<Enums.Colors>();
            foreach (var item in Enum.GetValues(typeof(Enums.Colors)))
            {
                if ((Enums.Colors)item != Enums.Colors.White)
                {
                    l.Add((Enums.Colors)item);
                }
            }
            for (int i = 0; i < numberOfColors; i++)
            {
                int index = i % l.Count;
                result.Add(GetRGBFromColor(l[index]));
            }
            return result;
        }

        private static List<Tuple<byte, byte, byte>> getSiliconSteelColors(int numberOfColors)
        {
            List<Tuple<byte, byte, byte>> result = new List<Tuple<byte, byte, byte>>();
            for (int i = 0; i < numberOfColors; i++)
            {
                double factor = i * 1.0 / numberOfColors;
                int gray = (int)Math.Floor(50 + 110 * factor);
                result.Add(new Tuple<byte, byte, byte>((byte)gray, (byte)gray, (byte)gray));
            }
            return result;
        }

        private static List<Tuple<byte, byte, byte>> getColorTransition(Tuple<byte, byte, byte> firstColor, Tuple<byte, byte, byte> secondColor, int numberOfColors)
        {
            List<Tuple<byte, byte, byte>> result = new List<Tuple<byte, byte, byte>>();
            //adds the first color
            result.Add(firstColor);
            double r1 = (double)firstColor.Item1;
            double r2 = (double)secondColor.Item1;
            double g1 = (double)firstColor.Item2;
            double g2 = (double)secondColor.Item2;
            double b1 = (double)firstColor.Item3;
            double b2 = (double)secondColor.Item3;
            double dr = ((double)r2 - (double)r1) / ((double)numberOfColors - 1);
            double dg = ((double)g2 - (double)g1) / ((double)numberOfColors - 1);
            double db = ((double)b2 - (double)b1) / ((double)numberOfColors - 1);
            for (int i = 1; i < numberOfColors - 1; i++)
            {
                double r = r1 + dr * i;
                double g = g1 + dg * i;
                double b = b1 + db * i;
                result.Add(new Tuple<byte, byte, byte>((byte)r, (byte)g, (byte)b));
            }
            //adds the last color (if it is the case)
            if (numberOfColors > 1) result.Add(secondColor);
            return result;
        }

        private static Tuple<Tuple<byte, byte, byte>, Tuple<byte, byte, byte>> getFirstAndLastColors(Enums.ColorTemplates colorTemplates)
        {
            Tuple<byte, byte, byte> c1 = null;
            Tuple<byte, byte, byte> c2 = null;
            switch (colorTemplates)
            {
                case Enums.ColorTemplates.Rainbow:
                case Enums.ColorTemplates.Cycle:
                case Enums.ColorTemplates.SiliconSteel:
                    return null;
                case Enums.ColorTemplates.GrayScale:
                    c1 = new Tuple<byte, byte, byte>(80, 80, 80);
                    c2 = new Tuple<byte, byte, byte>(240, 240, 240);
                    break;
                case Enums.ColorTemplates.RedScale:
                    c1 = new Tuple<byte, byte, byte>(240, 0, 0);
                    c2 = new Tuple<byte, byte, byte>(100, 40, 40);
                    break;
                case Enums.ColorTemplates.GreenScale:
                    c1 = new Tuple<byte, byte, byte>(0, 200, 0);
                    c2 = new Tuple<byte, byte, byte>(0, 50, 0);
                    break;
                case Enums.ColorTemplates.BlueScale:
                    c1 = new Tuple<byte, byte, byte>(0, 0, 240);
                    c2 = new Tuple<byte, byte, byte>(0, 0, 50);
                    break;
                case Enums.ColorTemplates.BlueToRed:
                    c1 = new Tuple<byte, byte, byte>(0, 0, 240);
                    c2 = new Tuple<byte, byte, byte>(240, 0, 0);
                    break;
                case Enums.ColorTemplates.RedToBlue:
                    c1 = new Tuple<byte, byte, byte>(240, 0, 0);
                    c2 = new Tuple<byte, byte, byte>(0, 0, 240);
                    break;
                case Enums.ColorTemplates.GreenToRed:
                    c1 = new Tuple<byte, byte, byte>(0, 200, 0);
                    c2 = new Tuple<byte, byte, byte>(240, 0, 0);
                    break;
                case Enums.ColorTemplates.RedToGreen:
                    c1 = new Tuple<byte, byte, byte>(240, 0, 0);
                    c2 = new Tuple<byte, byte, byte>(0, 200, 0);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return new Tuple<Tuple<byte, byte, byte>, Tuple<byte, byte, byte>>(c1, c2);
        }

        #endregion

        #region PUBLIC METHODS

        public static List<Tuple<byte, byte, byte>> GetRGBListFromColorTemplate(Enums.ColorTemplates colorTemplate, int numberOfColors)
        {
            List<Tuple<byte, byte, byte>> result = new List<Tuple<byte, byte, byte>>();
            switch (colorTemplate)
            {
                case Enums.ColorTemplates.Rainbow:
                    foreach (var item in getRainbowColors(numberOfColors))
                    {
                        result.Add(item);
                    }
                    break;
                case Enums.ColorTemplates.Cycle:
                    foreach (var item in getCycleColors(numberOfColors))
                    {
                        result.Add(item);
                    }
                    break;
                case Enums.ColorTemplates.GrayScale:
                case Enums.ColorTemplates.RedScale:
                case Enums.ColorTemplates.GreenScale:
                case Enums.ColorTemplates.BlueScale:
                case Enums.ColorTemplates.BlueToRed:
                case Enums.ColorTemplates.RedToBlue:
                case Enums.ColorTemplates.GreenToRed:
                case Enums.ColorTemplates.RedToGreen:
                    Tuple<Tuple<byte, byte, byte>, Tuple<byte, byte, byte>> refColors = getFirstAndLastColors(colorTemplate);
                    foreach (var item in getColorTransition(refColors.Item1, refColors.Item2, numberOfColors))
                    {
                        result.Add(item);
                    }
                    break;
                case Enums.ColorTemplates.SiliconSteel:
                    foreach (var item in getSiliconSteelColors(numberOfColors))
                    {
                        result.Add(item);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            return result;
        }

        public static Tuple<byte, byte, byte> GetRGBFromColor(Enums.Colors color)
        {
            switch (color)
            {
                case Enums.Colors.Red: return new Tuple<byte, byte, byte>(255, 0, 0);
                case Enums.Colors.Green: return new Tuple<byte, byte, byte>(0, 150, 0);
                case Enums.Colors.Blue: return new Tuple<byte, byte, byte>(0, 0, 255);
                case Enums.Colors.Black: return new Tuple<byte, byte, byte>(0, 0, 0);
                case Enums.Colors.Gray: return new Tuple<byte, byte, byte>(160, 160, 160);
                case Enums.Colors.White: return new Tuple<byte, byte, byte>(255, 255, 255);
                default:
                    throw new Exception("Unknow color");
            }
        }

        public static string GetRGBDescriptionFromColor(Enums.Colors color)
        {
            Tuple<byte, byte, byte> rgb = ColorTemplateController.GetRGBFromColor(color);
            return string.Format("{0}|{1}|{2}", rgb.Item1, rgb.Item2, rgb.Item3);
        }

        public static Tuple<byte, byte, byte> GetRGBFromRGBDescription(string rgbDescription)
        {
            byte[] rgbResult = new byte[4];
            string[] vars = rgbDescription.Split('|');
            for (int i = 0; i < vars.Length; i++)
            {
                byte b;
                if (byte.TryParse(vars[i], out b))
                {
                    rgbResult[i] = b;
                }
            }
            return new Tuple<byte, byte, byte>(rgbResult[0], rgbResult[1], rgbResult[2]);
        }

        public static OxyColor GetOxyColorFromRGBDescription(string rgbDescription)
        {
            byte[] rgbResult = new byte[4];
            string[] vars = rgbDescription.Split('|');
            for (int i = 0; i < vars.Length; i++)
            {
                byte b;
                if (byte.TryParse(vars[i], out b))
                {
                    rgbResult[i] = b;
                }
            }
            return OxyColor.FromRgb(rgbResult[0], rgbResult[1], rgbResult[2]);
        }

        public static bool ValidateRGBDescription(string variable)
        {
            bool result = true;
            string[] vars = variable.Split('|');
            if (vars.Length != 3) return false;
            for (int i = 0; i < vars.Length; i++)
            {
                byte b;
                if (!byte.TryParse(vars[i], out b))
                {
                    result = false;
                }
            }
            return result;
        }

        #endregion

    }
}
