﻿using Rhino;
using System;
using System.Collections.Generic;

namespace SAM 
 // namespace  SAM.Analytical.Grasshopper
{
    public static partial class Modify
    {
        [Obsolete("Obsolete since 2021.11.24")]
        public static void BakeGeometry_ByType(this RhinoDoc rhinoDoc, global::Grasshopper.Kernel.Data.IGH_Structure gH_Structure, bool cutOpenings = false, double tolerance = Core.Tolerance.Distance)
        {
            if (rhinoDoc == null)
                return;

            List<IPartition> partitions = new List<IPartition>();
            foreach (var variable in gH_Structure.AllData(true))
            {
                if (variable is GooPartition)
                {
                    partitions.Add(((GooPartition)variable).Value);
                }
                else if (variable is GooBuildingModel)
                {
                    BuildingModel buildingModel = ((GooBuildingModel)variable).Value;
                    if (buildingModel != null)
                    {
                        List<IPartition> hostPartitions_Temp = buildingModel.GetObjects<IPartition>();
                        if (hostPartitions_Temp != null && hostPartitions_Temp.Count > 0)
                        {
                            partitions.AddRange(hostPartitions_Temp);
                        }
                    }
                }
            }

            Rhino.Modify.BakeGeometry_ByType(rhinoDoc, partitions, cutOpenings, tolerance);
        }
    }
}