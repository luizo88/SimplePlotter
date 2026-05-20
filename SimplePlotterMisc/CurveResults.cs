using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimplePlotterMisc
{
    public static class CurveResults
    {
        public static double GetMinimum(List<PointObj> pointList)
        {
            double result = pointList.Min(x => x.Y);
            return result;
        }

        public static double GetAverage(List<PointObj> pointList)
        {
            double result = pointList.Average(x => x.Y);
            return result;
        }

        public static double GetMaximum(List<PointObj> pointList)
        {
            double result = pointList.Max(x => x.Y);
            return result;
        }

        public static double GetRMS(List<PointObj> pointList)
        {
            double result = Math.Sqrt(pointList.Average(x => Math.Pow(x.Y, 2)));
            return result;
        }

    }
}
