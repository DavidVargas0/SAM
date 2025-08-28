﻿// using SAM.Geometry.Planar;
// using SAM.Geometry.Spatial;

namespace SAM 
 // namespace  SAM.Analytical
{
    public static partial class Query
    {
        public static bool ApertureHost(this Panel panel, IClosedPlanar3D closedPlanar3D, double minArea = Core.Tolerance.MacroDistance, double maxDistance = Core.Tolerance.MacroDistance, double tolerance = Core.Tolerance.Distance)
        {
            if (closedPlanar3D == null)
            {
                return false;
            }

            Plane plane = panel?.PlanarBoundary3D?.Plane;
            if (plane == null)
            {
                return false;
            }

            if (panel.PanelType == Analytical.PanelType.Air)
            {
                return false;
            }

            Plane plane_closedPlanar3D = closedPlanar3D.GetPlane();
            if (plane_closedPlanar3D == null)
            {
                return false;
            }
            //TO DO: 2023-08-10 Temporary solution for protect from projecting aperture to wrong panel
            if (plane.Normal.Perpendicular(plane_closedPlanar3D.Normal, tolerance))
            {
                return false;
            } 

            Point3D point3D_ClosedPlanar3D = plane_closedPlanar3D.Origin;
            Point3D point3D_ClosedPlanar3D_Projected = plane.Project(plane_closedPlanar3D.Origin);

            if (point3D_ClosedPlanar3D.Distance(point3D_ClosedPlanar3D_Projected) >= maxDistance + tolerance)
            {
                return false;
            }

            double max = System.Math.Max(maxDistance, tolerance);

            BoundingBox3D boundingBox3D_Panel = panel.GetBoundingBox(max);
            if (boundingBox3D_Panel == null)
            {
                return false;
            }

            BoundingBox3D boundingBox3D_ClosedPlanar3D = closedPlanar3D.GetBoundingBox(max);
            if (boundingBox3D_ClosedPlanar3D == null)
            {
                return false;
            }

            if (!boundingBox3D_ClosedPlanar3D.InRange(boundingBox3D_Panel))
                return false;

            IClosedPlanar3D closedPlanar3D_Projected = plane.Project(closedPlanar3D);
            if (closedPlanar3D_Projected == null)
            {
                return false;
            }

            IClosed2D closed2D_Aperture = plane.Convert(closedPlanar3D_Projected);

            if (minArea != 0 && closed2D_Aperture.GetArea() <= minArea)
            {
                return false;
            }

            return true;
        }
    }
}