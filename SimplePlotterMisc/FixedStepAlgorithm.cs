using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimplePlotterMisc
{
    //https://rosettacode.org/wiki/Ramer-Douglas-Peucker_line_simplification
    public static class FixedStepAlgorithm
    {
        public static List<PointObj> FixStep(List<PointObj> pointList, double desiredNumberOfPoints)
        {
            List<PointObj> result = new List<PointObj>();
            if (pointList.Count < 2)
            {
                throw new ArgumentOutOfRangeException("Not enough points to transform");
            }
            //find first and last X and computes the step
            double t1 = pointList.First().X;
            double t2 = pointList.Last().X;
            double step = (t2 - t1) / (desiredNumberOfPoints - 1);
            //save the first point
            result.Add(new PointObj(pointList.First().X, pointList.First().Y));
            //compute and save intermediate points
            int cont = 0;
            double currentX = pointList[cont].X;
            double x0 = currentX;
            double currentY = pointList[cont].Y;
            double nextX = pointList[cont + 1].X;
            for (int i = 1; i < desiredNumberOfPoints - 1; i++)
            {
                double newX = x0 + i * step;
                //check if it changed the pair
                if (newX > nextX)
                {
                    cont++;
                    currentX = pointList[cont].X;
                    currentY = pointList[cont].Y;
                    nextX = pointList[cont + 1].X;
                }
                //check if it didn't passed to other pair
                do
                {
                    if (newX > nextX)
                    {
                        cont++;
                        currentX = pointList[cont].X;
                        currentY = pointList[cont].Y;
                        nextX = pointList[cont + 1].X;
                    }
                } while (newX > nextX);
                //interpolates
                double x1 = currentX;
                double y1 = currentY;
                double x2 = nextX;
                double y2 = pointList[cont + 1].Y;
                double newY = y1 + (y2 - y1) / (x2 - x1) * (newX - x1);
                result.Add(new PointObj(newX, newY));
            }
            //save last point
            result.Add(new PointObj(pointList.Last().X, pointList.Last().Y));
            return result;
        }
    }
}
