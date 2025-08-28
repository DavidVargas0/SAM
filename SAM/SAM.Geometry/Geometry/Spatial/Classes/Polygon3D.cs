﻿using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace SAM 
 // namespace  SAM.Geometry.Spatial
{
    public class Polygon3D : SAMGeometry, IClosedPlanar3D, ISegmentable3D
    {
        private List<Planar.Point2D> points;
        private Plane plane;

        public Polygon3D(Plane plane, IEnumerable<Planar.Point2D> points)
        {
            this.plane = plane;

            if (points != null)
                this.points = new List<Planar.Point2D>(points);
        }

        public Polygon3D(IEnumerable<Point3D> point3Ds, double tolerance = Core.Tolerance.Distance)
        {
            if (point3Ds != null)
            {
                List<Point3D> point3Ds_Temp = new List<Point3D>(point3Ds);
                //check if points first and last are the same
                if (point3Ds_Temp.First().Distance(point3Ds_Temp.Last()) <= tolerance)
                    point3Ds_Temp.RemoveAt(point3Ds_Temp.Count - 1);

                plane = Create.Plane(point3Ds_Temp, tolerance);
                if (plane != null)
                    points = point3Ds_Temp.ConvertAll(x => plane.Convert(x));
            }
        }

        public Polygon3D(Polygon3D polygon3D)
        {
            points = polygon3D.points.ConvertAll(x => new Planar.Point2D(x));
            plane = new Plane(polygon3D.plane);
        }

        public Polygon3D(JObject jObject)
            : base(jObject)
        {
        }

        public List<Point3D> GetPoints()
        {
            if(plane == null)
            {
                return null;
            }

            return points?.ConvertAll(x => plane.Convert(x));
        }

        public Plane GetPlane()
        {
            if(plane == null)
            {
                return null;
            }
            
            return new Plane(plane);
        }

        public override ISAMGeometry Clone()
        {
            return new Polygon3D(this);
        }

        /// <summary>
        /// Inserts new point on one of the edges (closest to given point3D)
        /// </summary>
        /// <param name="point3D"> Point2D will be use as a reference to insert Point3D on Polygon3D edge</param>
        /// <param name="tolerance">tolerance</param>
        /// <returns>Point2D on Polygon2D edge</returns>
        public Point3D InsertClosest(Point3D point3D, double tolerance = Core.Tolerance.Distance)
        {
            return plane.Convert(Planar.Modify.InsertClosest(points, plane.Convert(plane.Project(point3D)), true, tolerance));
        }

        public List<Segment3D> GetSegments()
        {
            List<Point3D> point3Ds = GetPoints();

            int count = point3Ds.Count;

            Segment3D[] result = new Segment3D[count];
            for (int i = 0; i < count - 1; i++)
                result[i] = new Segment3D(point3Ds[i], point3Ds[i + 1]);

            result[count - 1] = new Segment3D(new Point3D(point3Ds[count - 1]), new Point3D(point3Ds[0]));

            return result.ToList();
        }

        public BoundingBox3D GetBoundingBox(double offset = 0)
        {
            return new BoundingBox3D(GetPoints(), offset);
        }

        public double GetPerimiter()
        {
            List<Planar.Segment2D> segment2Ds = Planar.Create.Segment2Ds(points, true);
            if (segment2Ds == null || segment2Ds.Count == 0)
                return double.NaN;

            double permieter = 0;
            segment2Ds.ForEach(x => permieter += x.GetLength());

            return permieter;
        }

        public Face3D ToFace3D()
        {
            return new Face3D(this);
        }

        public IClosed3D GetExternalEdge()
        {
            return new Polygon3D(this);
        }

        public List<ICurve3D> GetCurves()
        {
            return GetSegments().ConvertAll(x => (ICurve3D)x);
        }

        public ISAMGeometry3D GetMoved(Vector3D vector3D)
        {
            List<Point3D> point3Ds = GetPoints();

            return new Polygon3D(point3Ds.ConvertAll(x => (Point3D)x.GetMoved(vector3D)));
        }

        public ISAMGeometry3D GetTransformed(Transform3D transform3D)
        {
            if (transform3D == null)
            {
                return null;
            }

            return Query.Transform(this, transform3D);
        }

        public double GetArea()
        {
            if (points == null || points.Count < 3)
                return 0;

            return Planar.Query.Area(points);
        }

        public bool Inside(Polygon3D polygon3D, double tolerance = Core.Tolerance.Distance)
        {
            Plane plane_1 = plane;
            Plane plane_2 = polygon3D.plane;

            if (!plane_1.Coplanar(plane_2, tolerance))
                return false;

            return polygon3D.points.TrueForAll(x => Planar.Query.Inside(points, x));
        }

        public void Reverse()
        {
            List<Point3D> point3Ds = GetPoints();
            point3Ds.Reverse();

            plane = new Plane(Query.Average(point3Ds), plane.Normal);
            points = point3Ds.ConvertAll(x => plane.Convert(x));

            //this.plane = Create.Plane(point3Ds);
            //this.points = point3Ds.ConvertAll(x => this.plane.Convert(x));
        }

        public override bool FromJObject(JObject jObject)
        {
            plane = Geometry.Create.ISAMGeometry<Plane>(jObject.Value<JObject>("Plane"));
            points = Geometry.Create.ISAMGeometries<Planar.Point2D>(jObject.Value<JArray>("Points"));
            return true;
        }

        public override JObject ToJObject()
        {
            JObject jObject = base.ToJObject();
            if (jObject == null)
                return null;

            jObject.Add("Points", Geometry.Create.JArray(points));
            jObject.Add("Plane", plane.ToJObject());
            return jObject;
        }

        public bool On(Point3D point3D, double tolerance = Core.Tolerance.Distance)
        {
            return Query.On(this, point3D, tolerance);
        }

        public Point3D GetCentroid()
        {
            return plane?.Convert(Planar.Query.Centroid(points));
        }

        public double GetLength()
        {
            return GetPerimiter();
        }

        public static implicit operator Polygon3D(Rectangle3D rectangle3D)
        {

            Plane plane = rectangle3D?.GetPlane();
            if(plane == null)
            {
                return null;
            }

            return new Polygon3D(plane, rectangle3D.GetPoints().ConvertAll(x => plane.Convert(x)));
        }

        public static implicit operator Polygon3D(Triangle3D triangle3D)
        {
            Plane plane = triangle3D?.GetPlane();
            if (plane == null)
            {
                return null;
            }

            return new Polygon3D(plane, triangle3D.GetPoints().ConvertAll(x => plane.Convert(x)));
        }
    }
}