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

        #region PUBLIC METHODS

        public static List<Tuple<byte, byte, byte>> GetRGBListFromColorTemplate(Enums.ColorTemplates colorTemplate, int numberOfColors)
        {
            List<Tuple<byte, byte, byte>> result = new List<Tuple<byte, byte, byte>>();
            Tuple<Tuple<byte, byte, byte>, Tuple<byte, byte, byte>> refColors = getFirstAndLastColors(colorTemplate);
            switch (colorTemplate)
            {
                case Enums.ColorTemplates.Rainbow:
                    break;
                case Enums.ColorTemplates.Cycle:
                    break;
                case Enums.ColorTemplates.GrayScale:
                case Enums.ColorTemplates.RedScale:
                case Enums.ColorTemplates.GreenScale:
                case Enums.ColorTemplates.BlueScale:
                case Enums.ColorTemplates.BlueToRed:
                case Enums.ColorTemplates.RedToBlue:
                case Enums.ColorTemplates.GreenToRed:
                case Enums.ColorTemplates.RedToGreen:
                    foreach (var item in getColorTransition(refColors.Item1, refColors.Item2, numberOfColors))
                    {
                        result.Add(item);
                    }
                    break;
                default:
                    break;
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

        private static Tuple<Tuple<byte,byte,byte>, Tuple<byte, byte, byte>> getFirstAndLastColors(Enums.ColorTemplates colorTemplates)
        {
            Tuple<byte, byte, byte> c1 = null;
            Tuple<byte, byte, byte> c2 = null;
            switch (colorTemplates)
            {
                case Enums.ColorTemplates.Rainbow:
                    break;
                case Enums.ColorTemplates.Cycle:
                    break;
                case Enums.ColorTemplates.GrayScale:
                    c1 = new Tuple<byte, byte, byte>(240, 240, 240);
                    c2 = new Tuple<byte, byte, byte>(20, 20, 20);
                    break;
                case Enums.ColorTemplates.RedScale:
                    c1 = new Tuple<byte, byte, byte>(240, 0, 0);
                    c2 = new Tuple<byte, byte, byte>(50, 0, 0);
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
                    break;
            }
            return new Tuple<Tuple<byte, byte, byte>, Tuple<byte, byte, byte>>(c1, c2);
        }

        #endregion

    }
}
