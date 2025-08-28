﻿using System.Collections.Generic;

namespace SAM 
 // namespace  SAM.Analytical
{
    public static partial class Query
    {
        public static double Sum(this AdjacencyCluster adjacencyCluster, Zone zone, string name)
        {
            if (adjacencyCluster == null || zone == null || string.IsNullOrWhiteSpace(name))
                return double.NaN;

            List<Space> spaces = adjacencyCluster.GetSpaces(zone);
            if (spaces == null)
                return double.NaN;

            double result = 0;
            bool contains = false;
            foreach (Space space in spaces)
            {
                if (!space.TryGetValue(name, out double value) || double.IsNaN(value))
                    continue;

                result += value;
                contains = true;
            }

            if (!contains)
                return double.NaN;

            return result;
        }

        public static double Sum(this AdjacencyCluster adjacencyCluster, Zone zone, SpaceParameter spaceParameter)
        {
            if (adjacencyCluster == null || zone == null)
                return double.NaN;

            List<Space> spaces = adjacencyCluster.GetSpaces(zone);
            if (spaces == null)
                return double.NaN;

            double result = 0;
            bool contains = false;
            foreach (Space space in spaces)
            {
                if (!space.TryGetValue(spaceParameter, out double value) || double.IsNaN(value))
                    continue;

                result += value;
                contains = true;
            }

            if (!contains)
                return double.NaN;

            return result;
        }

        public static double Sum(this IEnumerable<SpaceSimulationResult> spaceSimulationResults, SpaceSimulationResultParameter spaceSimulationResultParameter)
        {
            if (spaceSimulationResults == null)
                return double.NaN;

            double result = 0;
            bool contains = false;
            foreach (SpaceSimulationResult spaceSimulationResult in spaceSimulationResults)
            {
                if (!spaceSimulationResult.TryGetValue(spaceSimulationResultParameter, out double value) || double.IsNaN(value))
                    continue;

                result += value;
                contains = true;
            }

            if (!contains)
                return double.NaN;

            return result;
        }
    }
}