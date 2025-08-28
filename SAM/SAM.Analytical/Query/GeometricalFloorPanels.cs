﻿using SAM.Geometry.Spatial;
using System.Collections.Generic;

namespace  SAM.Analytical
{
    public static partial class Query
    {
        public static List<Panel> GeomericalFloorPanels(this AdjacencyCluster adjacencyCluster, Space space, double maxTiltDifference = 20, double silverSpacing = Core.Tolerance.MacroDistance, double tolerance = Core.Tolerance.Distance)
        {
            if (adjacencyCluster == null || space == null)
                return null;

            Dictionary<IPanel, Vector3D> dictionary = adjacencyCluster.NormalDictionary(space, out Shell shell, true, silverSpacing, tolerance);
            if (dictionary == null || dictionary.Count == 0)
                return null;

            List<Panel> result = new List<Panel>();
            foreach(KeyValuePair<IPanel, Vector3D> keyValuePair in dictionary)
            {
                Vector3D vector3D = keyValuePair.Value;
                if (vector3D == null)
                    continue;

                Panel panel = keyValuePair.Key as Panel;
                if (panel == null)
                    continue;

                if (Vector3D.WorldZ.SameHalf(vector3D))
                    continue;

                double tilt = Geometry.Spatial.Query.Tilt(vector3D);
                if (double.IsNaN(tilt))
                    continue;

                if (180 - maxTiltDifference > tilt || tilt > 180 + maxTiltDifference)
                    continue;

                result.Add(panel);
            }

            return result;
        }
    }
}