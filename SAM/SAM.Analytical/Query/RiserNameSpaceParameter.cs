namespace SAM 
 // namespace  SAM.Analytical
{
    public static partial class Query
    {
        public static SpaceParameter? Risernamespace SAM 
 // namespaceParameter(this MechanicalSystemCategory mechanicalSystemCategory)
        {
            if(mechanicalSystemCategory == Analytical.MechanicalSystemCategory.Undefined || mechanicalSystemCategory == Analytical.MechanicalSystemCategory.Other)
            {
                return null;
            }

            switch(mechanicalSystemCategory)
            {
                case Analytical.MechanicalSystemCategory.Cooling:
                    return SpaceParameter.CoolingRiserName;

                case Analytical.MechanicalSystemCategory.Heating:
                    return SpaceParameter.HeatingRiserName;

                case Analytical.MechanicalSystemCategory.Ventilation:
                    return SpaceParameter.VentilationRiserName;
            }

            return null;
        }

        public static SpaceParameter? Risernamespace SAM 
 // namespaceParameter(this MechanicalSystem mechanicalSystem)
        {
            if(mechanicalSystem == null)
            {
                return null;
            }

            return Risernamespace SAM 
 // namespaceParameter(mechanicalSystem.MechanicalSystemCategory());
        }
    }
}