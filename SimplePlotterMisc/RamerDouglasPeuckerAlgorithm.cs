using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePlotterMisc
{
    //https://rosettacode.org/wiki/Ramer-Douglas-Peucker_line_simplification
    public static class RamerDouglasPeuckerAlgorithm
    {
        public static double PerpendicularDistance(PointObj pt, PointObj lineStart, PointObj lineEnd)
        {
            double dx = lineEnd.X - lineStart.X;
            double dy = lineEnd.Y - lineStart.Y;

            // Normalize
            double mag = Math.Sqrt(dx * dx + dy * dy);
            if (mag > 0.0)
            {
                dx /= mag;
                dy /= mag;
            }
            double pvx = pt.X - lineStart.X;
            double pvy = pt.Y - lineStart.Y;

            // Get dot product (project pv onto normalized direction)
            double pvdot = dx * pvx + dy * pvy;

            // Scale line direction vector and subtract it from pv
            double ax = pvx - pvdot * dx;
            double ay = pvy - pvdot * dy;

            return Math.Sqrt(ax * ax + ay * ay);
        }

        public static List<PointObj> RamerDouglasPeucker(List<PointObj> pointList, double epsilon)
        {
            List<PointObj> result = new List<PointObj>();

            if (pointList.Count < 2)
            {
                throw new ArgumentOutOfRangeException("Not enough points to simplify");
            }

            // Find the point with the maximum distance from line between the start and end
            double dmax = 0.0;
            int index = 0;
            int end = pointList.Count - 1;
            for (int i = 1; i < end; ++i)
            {
                double d = PerpendicularDistance(pointList[i], pointList[0], pointList[end]);
                if (d > dmax)
                {
                    index = i;
                    dmax = d;
                }
            }

            // If max distance is greater than epsilon, recursively simplify
            if (dmax > epsilon)
            {
                List<PointObj> recResults1 = new List<PointObj>();
                List<PointObj> recResults2 = new List<PointObj>();
                List<PointObj> firstLine = pointList.Take(index + 1).ToList();
                List<PointObj> lastLine = pointList.Skip(index).ToList();
                recResults1 = RamerDouglasPeucker(firstLine, epsilon);
                recResults2 = RamerDouglasPeucker(lastLine, epsilon);

                // build the result list
                result.AddRange(recResults1.Take(recResults1.Count - 1));
                result.AddRange(recResults2);
                if (result.Count < 2) throw new Exception("Problem assembling output");
            }
            else
            {
                // Just return start and end points
                result.Clear();
                result.Add(pointList[0]);
                result.Add(pointList[pointList.Count - 1]);
            }

            return result;
        }
    }
}
