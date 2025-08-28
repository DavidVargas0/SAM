﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SAM 
 // namespace  SAM.Geometry.Spatial
{
    public static partial class Query
    {
        public static Dictionary<Face3D, Point3D> IntersectionDictionary(this Point3D point3D, Vector3D vector3D, IEnumerable<Face3D> face3Ds, bool keepDirection, bool sort = true, double tolerance = Core.Tolerance.Distance)
        {
            if (face3Ds == null || point3D == null || vector3D == null)
                return null;

            List<Tuple<Point3D, Face3D>> tuples= new List<Tuple<Point3D, Face3D>>();
            List<Point3D> point3Ds = new List<Point3D>();
            foreach (Face3D face3D in face3Ds)
            {
                PlanarIntersectionResult planarIntersectionResult = Create.PlanarIntersectionResult(face3D, point3D, vector3D, tolerance);
                if (planarIntersectionResult == null || !planarIntersectionResult.Intersecting)
                    continue;

                Point3D point3D_Intersection = planarIntersectionResult.GetGeometry3Ds<Point3D>()?.FirstOrDefault();
                if (point3D_Intersection == null)
                    continue;

                point3Ds.Add(point3D_Intersection);
                tuples.Add(new Tuple<Point3D, Face3D>(point3D_Intersection, face3D));
            }

            if (keepDirection)
            {
                point3Ds.RemoveAll(x => !(new Vector3D(point3D, x)).SameHalf(vector3D));
            }

            if (sort)
            {
                Modify.SortByDistance(point3Ds, point3D);
            }



            Dictionary<Face3D, Point3D> result = new Dictionary<Face3D, Point3D>();
            foreach(Point3D point3D_Temp in point3Ds)
            {
                foreach (Tuple<Point3D, Face3D> tuple in tuples.FindAll(x => x.Item1 == point3D_Temp))
                    result[tuple.Item2] = tuple.Item1;
            }

            return result;
        }
    }
}