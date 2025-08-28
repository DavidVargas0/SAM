﻿using SAM.Math;
using System.Collections.Generic;
using System.Linq;

namespace  SAM.Geometry.Planar
{
    public static partial class Query
    {
        public static List<Point2D> Intersections(this ISegmentable2D segmentable2D_1, ISegmentable2D segmentable2D_2, double tolerance = Core.Tolerance.Distance)
        {
            if (segmentable2D_1 == null || segmentable2D_2 == null)
                return null;

            List<Segment2D> segment2Ds_1 = segmentable2D_1.GetSegments();
            if (segment2Ds_1 == null)
                return null;

            List<Segment2D> segment2Ds_2 = segmentable2D_2.GetSegments();
            if (segment2Ds_2 == null)
                return null;

            List<Point2D> result = new List<Point2D>();
            foreach (Segment2D segment2D_1 in segment2Ds_1)
            {
                if (segment2D_1 == null)
                    continue;

                foreach (Segment2D segment2D_2 in segment2Ds_2)
                {
                    if (segment2D_2 == null)
                        continue;

                    Point2D point2D_Intersection = segment2D_1.Intersection(segment2D_2, true, tolerance);
                    if (point2D_Intersection == null)
                        continue;

                    result.Add(point2D_Intersection, tolerance);
                }
            }

            return result;
        }

        public static List<Point2D> Intersections(this ISegmentable2D segmentable2D, IEnumerable<ISegmentable2D> segmentable2Ds, double tolerance = Core.Tolerance.Distance)
        {
            if (segmentable2D == null || segmentable2Ds == null)
                return null;

            List<Segment2D> segment2Ds_Segmentable2D = segmentable2D.GetSegments();
            if (segment2Ds_Segmentable2D == null)
                return null;

            List<Point2D> point2Ds = new List<Point2D>();
            foreach (ISegmentable2D segmentable2D_Temp in segmentable2Ds)
            {
                List<Point2D> point2Ds_Temp = Intersections(segmentable2D, segmentable2D_Temp, tolerance);
                if (point2Ds_Temp == null || point2Ds_Temp.Count == 0)
                    continue;

                foreach(Point2D point2D_Temp in point2Ds_Temp)
                {
                    Point2D point2D = point2Ds.Find(x => x.AlmostEquals(point2D_Temp, tolerance));
                    if (point2D == null)
                        point2Ds.Add(point2D_Temp);
                }
            }

            return point2Ds;
        }

        public static List<Point2D> Intersections(this ISegmentable2D segmentable2D, Line2D line2D, double tolerance = Core.Tolerance.Distance)
        {
            if(line2D == null)
            {
                return null;
            }

            List<Segment2D> segment2Ds = segmentable2D?.GetSegments();
            if(segment2Ds == null)
            {
                return null;
            }

            List<Point2D> result = new List<Point2D>();
            foreach(Segment2D segment2D in segment2Ds)
            {
                Point2D point2D = line2D.Intersection(segment2D, tolerance);
                if(point2D == null)
                {
                    continue;
                }

                result.Add(point2D, tolerance);
            }

            return result;
        }

        public static List<Point2D> Intersections(this Point2D point2D, Vector2D vector2D, ISegmentable2D segmentable2D, bool keepDirection, bool removeCollinear = true, bool sort = true, bool selfIntersection = true, double tolerance = Core.Tolerance.Distance)
        {
            if (point2D == null || vector2D == null)
                return null;

            return IntersectionDictionary(point2D, vector2D, segmentable2D, keepDirection, removeCollinear, sort, selfIntersection, tolerance)?.Keys?.ToList();
        }

        public static List<Point2D> Intersections(this Point2D point2D, Vector2D vector2D, IEnumerable<ISegmentable2D> segmentable2Ds, bool keepDirection, bool removeCollinear = true, bool sort = true, bool selfIntersection = true, double tolerance = Core.Tolerance.Distance)
        {
            if (point2D == null || vector2D == null)
                return null;

            return IntersectionDictionary(point2D, vector2D, Segment2Ds(segmentable2Ds), keepDirection, removeCollinear, sort, selfIntersection, tolerance)?.Keys?.ToList();
        }

        public static List<Point2D> Intersections(this PolynomialEquation polynomialEquation_1, PolynomialEquation polynomialEquation_2)
        {
            if (polynomialEquation_1 == null || polynomialEquation_2 == null)
            {
                return null;
            }

            MathNet.Numerics.Polynomial polynomial_1 = Math.Convert.ToMathNet(polynomialEquation_1);
            if (polynomial_1 == null)
            {
                return null;
            }

            MathNet.Numerics.Polynomial polynomial_2 = Math.Convert.ToMathNet(polynomialEquation_2);
            if (polynomial_2 == null)
            {
                return null;
            }

            MathNet.Numerics.Polynomial polynomial = polynomial_1 - polynomial_2;
            if (polynomial == null)
            {
                return null;
            }

            System.Numerics.Complex[] complexes = polynomial.Roots();
            if (complexes == null)
            {
                return null;
            }

            List<Point2D> result = new List<Point2D>();
            foreach (System.Numerics.Complex complex in complexes)
            {
                Point2D point2D = complex.ToSAM();
                if (point2D == null)
                {
                    continue;
                }

                result.Add(point2D);
            }

            return result;
        }
    }
}