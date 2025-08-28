﻿using Rhino;
using System.Collections.Generic;

namespace  SAM.Analytical.Grasshopper
{
    public static partial class Modify
    {
        public static void BakeGeometry_ByConstruction(this RhinoDoc rhinoDoc, global::Grasshopper.Kernel.Data.IGH_Structure gH_Structure, bool cutApertures = false, double tolerance = Core.Tolerance.Distance)
        {
            if (rhinoDoc == null)
                return;

            List<IPanel> panels = new List<IPanel>();
            foreach (var variable in gH_Structure.AllData(true))
            {
                if (variable is GooPanel)
                {
                    panels.Add(((GooPanel)variable).Value);
                }
                else if (variable is GooAdjacencyCluster)
                {
                    List<IPanel> panels_Temp = ((GooAdjacencyCluster)variable).Value?.GetObjects<IPanel>();
                    if (panels_Temp != null && panels_Temp.Count != 0)
                    {
                        panels.AddRange(panels_Temp);
                    }
                }
                else if (variable is GooAnalyticalModel)
                {
                    List<IPanel> panels_Temp = ((GooAnalyticalModel)variable).Value?.AdjacencyCluster.GetObjects<IPanel>();
                    if (panels_Temp != null && panels_Temp.Count != 0)
                    {
                        panels.AddRange(panels_Temp);
                    }
                }
            }

            Rhino.Modify.BakeGeometry_ByConstruction(rhinoDoc, panels, cutApertures, tolerance);
        }

    }
}