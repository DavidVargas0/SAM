﻿using Newtonsoft.Json.Linq;

using System;

namespace SAM 
 // namespace  SAM.Core
{
    public class GasMaterial : FluidMaterial
    {
        public GasMaterial(string name)
            : base(name)
        {

        }

        public GasMaterial(Guid guid, string name, string displayName, string description, double thermalConductivity, double density, double specificHeatCapacity, double dynamicViscosity)
        : base(guid, name, displayName, description, thermalConductivity, density, specificHeatCapacity, dynamicViscosity)
        {

        }

        public GasMaterial(string name, string group, string displayName, string description, double thermalConductivity, double specificHeatCapacity, double density, double dynamicViscosity)
            : base(name, group, displayName, description, thermalConductivity, specificHeatCapacity, density, dynamicViscosity)
        {

        }
        
        public GasMaterial(Guid guid, string name)
            : base(guid, name)
        {

        }

        public GasMaterial(JObject jObject)
            : base(jObject)
        {
        }

        public GasMaterial(GasMaterial gasMaterial)
            : base(gasMaterial)
        {

        }

        public GasMaterial(string name, Guid guid, GasMaterial gasMaterial, string displayName, string description)
            :base(name, guid, gasMaterial, displayName, description)
        {

        }

        public override bool FromJObject(JObject jObject)
        {
            if (!base.FromJObject(jObject))
                return false;

            return true;
        }

        public override JObject ToJObject()
        {
            JObject jObject = base.ToJObject();
            if (jObject == null)
                return jObject;

            return jObject;
        }
    }
}