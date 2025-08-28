﻿using System;
using System.Collections.Generic;

namespace  SAM.Geometry.Planar
{
    public static partial class Query
    {
        public static List<Face2D> SplitByInternalEdges(this Face2D face2D, double tolerance = Core.Tolerance.Distance)
        {
            if(face2D == null)
            {
                return null;
            }

            List<IClosed2D> internalEdges = face2D.InternalEdge2Ds;
            if (internalEdges == null || internalEdges.Count == 0)
            {
                return new List<Face2D>() { (Face2D)face2D.Clone() };
            }

            ISegmentable2D externalEdge = face2D.ExternalEdge2D as ISegmentable2D;
            if (externalEdge == null)
            {
                throw new NotImplementedException();
            }

            List<Point2D> point2Ds = externalEdge.GetPoints();
            if (point2Ds == null || point2Ds.Count < 3)
            {
                return null;
            }

            List<ISegmentable2D> segmentable2Ds = new List<ISegmentable2D>();
            foreach (IClosed2D internalEdge_Closed2D in internalEdges)
            {
                ISegmentable2D internalEdge = internalEdge_Closed2D as ISegmentable2D;
                if (internalEdge == null)
                {
                    throw new NotImplementedException();
                }

                List<Point2D> point2Ds_InternalEdge = internalEdge.GetPoints();
                if(point2Ds_InternalEdge == null || point2Ds_InternalEdge.Count == 0)
                {
                    continue;
                }

                segmentable2Ds.Add(internalEdge);

                foreach(Point2D point2D_InternalEdge in point2Ds_InternalEdge)
                {
                    Point2D point2D_Closest = Closest(externalEdge, point2Ds_InternalEdge);
                    if(point2D_Closest == null)
                    {
                        continue;
                    }

                    point2Ds.Add(point2D_Closest, tolerance);
                }
            }

            List<Segment2D> segment2Ds = externalEdge.GetSegments();

            foreach (ISegmentable2D segmentable2D in segmentable2Ds)
            {
                List<Tuple<Point2D, Point2D>> tuples = new List<Tuple<Point2D, Point2D>>();

                point2Ds.ForEach(x => tuples.Add(new Tuple<Point2D, Point2D>(Closest(segmentable2D, x), x)));

                if (tuples.Count < 3)
                {
                    continue;
                }
                
                tuples.Sort((x, y) => x.Item1.Distance(x.Item2).CompareTo(y.Item1.Distance(y.Item2)));

                List<Segment2D> segment2Ds_Tuples = new List<Segment2D>();
                foreach(Tuple<Point2D, Point2D> tuple in tuples)
                {
                    if (tuple.Item1.Distance(tuple.Item2) <= tolerance)
                    {
                        continue;
                    }

                    Segment2D segment2D = new Segment2D(tuple.Item1, tuple.Item2);
                    segment2Ds_Tuples.Add(segment2D);
                }

                Segment2D segment2D_1 = segment2Ds_Tuples[0];
                Segment2D segment2D_2 = segment2Ds_Tuples.Find(x => x.Distance(segment2D_1) > segment2D_1.GetLength());
                if(segment2D_2 == null)
                {
                    segment2D_2 = segment2Ds_Tuples.Find(x => x.Distance(segment2D_1) >= tolerance);
                }

                if (segment2D_2 == null)
                {
                    segment2D_2 = segment2Ds_Tuples[1];
                }

                segment2Ds.Add(segment2D_1);
                segment2Ds.Add(segment2D_2);
                segment2Ds.AddRange(segmentable2D.GetSegments());
            }

            List<Polygon2D> polygon2Ds = Create.Polygon2Ds(segment2Ds, tolerance);
            if (polygon2Ds == null)
            {
                return null;
            }

            polygon2Ds.RemoveAll(x => !face2D.Inside(x.InternalPoint2D()));

            return polygon2Ds.ConvertAll(x => new Face2D(x));
        }
    }
}