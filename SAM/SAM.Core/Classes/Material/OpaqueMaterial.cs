﻿using Newtonsoft.Json.Linq;

using System;

namespace SAM 
 // namespace  SAM.Core
{
    public class OpaqueMaterial : SolidMaterial
    {
        public OpaqueMaterial(string name)
            : base(name)
        {

        }

        public OpaqueMaterial(string name, string group, string displayName, string description, double thermalConductivity, double specificHeatCapacity, double density)
            : base(name, group, displayName, description, thermalConductivity, specificHeatCapacity, density)
        {

        }

        public OpaqueMaterial(Guid guid, string name)
            : base(guid, name)
        {

        }

        public OpaqueMaterial(string name, Guid guid, OpaqueMaterial opaqueMaterial, string displayName, string description)
            : base(name, guid, opaqueMaterial, displayName, description)
        {

        }

        public OpaqueMaterial(Guid guid, string name, string displayName, string description, double thermalConductivity, double density, double specificHeatCapacity)
            : base(guid, name, displayName, description, thermalConductivity, density, specificHeatCapacity)
        {

        }

        public OpaqueMaterial(JObject jObject)
            : base(jObject)
        {
        }

        public OpaqueMaterial(OpaqueMaterial opaqueMaterial)
            : base(opaqueMaterial)
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