namespace SAM 
 // namespace  SAM.Analytical
{
    public static partial class Query
    {
        public static double CalculatedVolume(this Space space, BuildingModel buildingModel = null, double silverSpacing = Tolerance.MacroDistance, double tolerance = Tolerance.Distance)
        {
            if (space.TryGetValue(SpaceParameter.Volume, out double result) && !double.IsNaN(result))
                return result;

            if (buildingModel == null)
                return double.NaN;

            return buildingModel.GetVolume(space, silverSpacing, tolerance);

        }
    }
}