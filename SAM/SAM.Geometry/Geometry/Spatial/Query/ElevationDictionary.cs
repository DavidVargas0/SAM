﻿using System;
using System.Collections.Generic;

namespace  SAM.Geometry.Spatial
{
    public static partial class Query
    {
        public static Dictionary<double, List<Face3D>> ElevationDictionary(this IEnumerable<Face3D> face3Ds, out double maxElevation, double tolerance = Core.Tolerance.Distance)
        {
            maxElevation = double.NaN;

            if (face3Ds == null)
                return null;

            List<Tuple<double, List<Face3D>>> tuples_Elevation = new List<Tuple<double, List<Face3D>>>();
            foreach (Face3D face3D in face3Ds)
            {
                BoundingBox3D boundingBox3D = face3D.GetBoundingBox();
                if (boundingBox3D == null)
                    continue;

                double minElevation = boundingBox3D.Min.Z;
                Tuple<double, List<Face3D>> tuple = tuples_Elevation.Find(x => System.Math.Abs(x.Item1 - minElevation) < tolerance);
                if (tuple == null)
                {
                    tuple = new Tuple<double, List<Face3D>>(minElevation, new List<Face3D>());
                    tuples_Elevation.Add(tuple);
                }

                double maxElevation_Temp = boundingBox3D.Max.Z;
                if (double.IsNaN(maxElevation) || maxElevation < maxElevation_Temp)
                {
                    maxElevation = maxElevation_Temp;
                }

                tuple.Item2.Add(face3D);
            }

            tuples_Elevation.Sort((x, y) => x.Item1.CompareTo(y.Item1));

            Dictionary<double, List<Face3D>> result = new Dictionary<double, List<Face3D>>();
            tuples_Elevation.ForEach(x => result[x.Item1] = x.Item2);

            return result;
        }

        public static Dictionary<double, List<Face3D>> ElevationDictionary(this IEnumerable<Face3D> face3Ds, double tolerance = Core.Tolerance.Distance)
        {
            return ElevationDictionary(face3Ds, out double maxElevation, tolerance);
        }
    }
}