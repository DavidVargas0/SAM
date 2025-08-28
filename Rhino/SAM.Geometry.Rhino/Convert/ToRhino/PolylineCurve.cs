﻿// using SAM.Geometry.Spatial;
using System.Collections.Generic;
using System.Linq;

namespace SAM 
 // namespace  SAM.Geometry.Rhino
{
    public static partial class Convert
    {
        public static global::Rhino.Geometry.PolylineCurve ToRhino_PolylineCurve(this IEnumerable<ICurve3D> curve3Ds)
        {
            if (curve3Ds == null || curve3Ds.Count() == 0)
                return null;

            List<global::Rhino.Geometry.Point3d> points = new List<global::Rhino.Geometry.Point3d>();
            points.Add(curve3Ds.First().GetStart().ToRhino());

            points.AddRange(curve3Ds.ToList().ConvertAll(x => x.GetEnd().ToRhino()));
           

            if (points == null || points.Count < 2)
                return null;

            return new global::Rhino.Geometry.PolylineCurve(points);
        }

        public static global::Rhino.Geometry.PolylineCurve ToRhino_PolylineCurve(this Polygon3D polygon3D)
        {
            List<global::Rhino.Geometry.Point3d> points = polygon3D?.GetPoints()?.ConvertAll(x => x.ToRhino());
            if (points == null || points.Count < 2)
            {
                return null;
            }

            points.Add(polygon3D.GetPoints().First().ToRhino());

            return new global::Rhino.Geometry.PolylineCurve(points);
        }

        public static global::Rhino.Geometry.PolylineCurve ToRhino_PolylineCurve(this Triangle3D triangle3D)
        {
            List<global::Rhino.Geometry.Point3d> points = triangle3D?.GetPoints()?.ConvertAll(x => x.ToRhino());
            if (points == null || points.Count < 2)
            {
                return null;
            }

            points.Add(triangle3D.GetPoints().First().ToRhino());

            return new global::Rhino.Geometry.PolylineCurve(points);
        }

        public static global::Rhino.Geometry.PolylineCurve ToRhino_PolylineCurve(this Polyline3D polyline3D, bool close)
        {
            List<global::Rhino.Geometry.Point3d> points = polyline3D?.Points?.ConvertAll(x => x.ToRhino());
            if (points == null || points.Count < 2)
            {
                return null;
            }

            if (close)
                points.Add(polyline3D.Points.First().ToRhino());

            return new global::Rhino.Geometry.PolylineCurve(points);
        }

        public static global::Rhino.Geometry.PolylineCurve ToRhino_PolylineCurve(this Planar.Polygon2D polygon2D)
        {
            List<global::Rhino.Geometry.Point3d> points = polygon2D.Points.ConvertAll(x => x.ToRhino());
            points.Add(polygon2D.Points.First().ToRhino());

            return new global::Rhino.Geometry.PolylineCurve(points);
        }

        public static global::Rhino.Geometry.PolylineCurve ToRhino_PolylineCurve(this global::Rhino.Geometry.Curve curve)
        {
            global::Rhino.Geometry.PolylineCurve polylineCurve = curve.ToPolyline(0, 0, 0.2, 0);
            return polylineCurve;
        }
    }
}