﻿using System.Collections.Generic;

namespace  SAM.Geometry.Spatial
{
    public static partial class Create
    {
        public static Rectangle3D Rectangle3D(this IClosedPlanar3D closedPlanar3D, double tolerance = Core.Tolerance.Distance)
        {
            if (closedPlanar3D == null)
                return null;

            IClosedPlanar3D closedPlanar3D_Temp = closedPlanar3D;
            if(closedPlanar3D_Temp is Face3D)
            {
                closedPlanar3D_Temp = ((Face3D)closedPlanar3D_Temp).GetExternalEdge3D();
            }

            Plane plane = closedPlanar3D_Temp?.GetPlane();
            if(plane == null)
            {
                return null;
            }

            Planar.ISegmentable2D segmentable2D = plane.Convert(closedPlanar3D_Temp) as Planar.ISegmentable2D;
            if(segmentable2D == null)
            {
                return null;
            }

            List<Planar.Point2D> point2Ds = segmentable2D.GetPoints();
            if(point2Ds == null || point2Ds.Count ==0)
            {
                return null;
            }

            Planar.Rectangle2D rectangle2D = Planar.Create.Rectangle2D(segmentable2D.GetPoints());
            if(rectangle2D == null)
            {
                return null;
            }

            return plane.Convert(rectangle2D);

        }

        public static Rectangle3D Rectangle3D(Segment3D segment3D, Vector3D vector3D, double tolerance = Core.Tolerance.Distance)
        {
            if (segment3D == null || vector3D == null)
            {
                return null;
            }

            double length_Segment3D = segment3D.GetLength();
            if(length_Segment3D < tolerance)
            {
                return null;
            }

            double length_Vector3D = vector3D.Length;
            if(length_Vector3D < tolerance)
            {
                return null;
            }

            Plane plane = Plane(segment3D[0], segment3D[1], (Point3D)segment3D[1].GetMoved(vector3D));
            if(plane == null && !plane.IsValid())
            {
                return null;
            }

            Planar.Point2D origin = plane.Convert(segment3D[0]);

            Planar.Rectangle2D rectangle2D = new Planar.Rectangle2D(origin, length_Segment3D, vector3D.Length);

            return new Rectangle3D(plane, rectangle2D);
        }
    }
}