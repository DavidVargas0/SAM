﻿using System.Collections.Generic;
using System.Linq;

namespace  SAM.Geometry.Planar
{
    public static partial class Modify
    {
        /// <summary>
        /// Inserts new point on one of the edges (closest to given point2D)
        /// </summary>
        /// <param name="point2Ds">Point2D list will be modified</param>
        /// <param name="point2D">Point2D will be inserted</param>
        /// <param name="close">Is Closed</param>
        /// <param name="tolerance">Tolerance</param>
        /// <returns>Inserted Point2D</returns>
        public static Point2D InsertClosest(this List<Point2D> point2Ds, Point2D point2D, bool close = false, double tolerance = Core.Tolerance.Distance)
        {
            List<Segment2D> segment2Ds = Create.Segment2Ds(point2Ds, close);
            if (segment2Ds == null || segment2Ds.Count == 0)
                return null;

            int index = -1;
            Point2D point2D_Closest = null;
            double distance_Min = double.MaxValue;

            for (int i = 0; i < segment2Ds.Count; i++)
            {
                Segment2D segment2D = segment2Ds[i];

                Point2D point2D_Closest_Temp = segment2D.Closest(point2D);
                double distance = point2D.Distance(point2D_Closest_Temp);
                if (distance < distance_Min)
                {
                    distance_Min = distance;
                    point2D_Closest = point2D_Closest_Temp;
                    index = i;
                }
            }

            if (index == -1)
                return null;

            Segment2D segment2D_Temp = segment2Ds[index];
            if (point2D_Closest.AlmostEquals(segment2D_Temp[0], tolerance))
                return segment2D_Temp[0];

            if(point2D_Closest.AlmostEquals(segment2D_Temp[1], tolerance))
                return segment2D_Temp[1];

            segment2Ds[index] = new Segment2D(segment2D_Temp[0], point2D_Closest);
            segment2Ds.Insert(index + 1, new Segment2D(point2D_Closest, segment2D_Temp[1]));

            point2Ds.Clear();
            point2Ds.AddRange(segment2Ds.ConvertAll(x => x.Start));
            if (!close)
                point2Ds.Add(segment2Ds.Last()[1]);

            return point2D_Closest;
        }
    }
}