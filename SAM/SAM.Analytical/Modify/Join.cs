// using SAM.Geometry.Spatial;
using System.Collections.Generic;

namespace SAM 
 // namespace  SAM.Analytical
{
    public static partial class Modify
    {
        public static void Join(this List<Panel> panels, double elevation, double distance, out List<Panel> panels_Extended, out List<Panel> panels_Trimmed, out List<Segment3D> segment3Ds, double snapTolerance = Tolerance.MacroDistance, double tolerance_Angle = Tolerance.Angle, double tolerance_Distance = Tolerance.Distance)
        {
            panels_Extended = null;
            panels_Trimmed = null;
            segment3Ds = null;

            if (panels == null)
                return;

            Extend(panels, elevation, distance, out panels_Extended, out segment3Ds, snapTolerance, tolerance_Angle, tolerance_Distance);
            Trim(panels, elevation, distance, out panels_Trimmed, out segment3Ds, snapTolerance, tolerance_Distance);
        }
    }
}