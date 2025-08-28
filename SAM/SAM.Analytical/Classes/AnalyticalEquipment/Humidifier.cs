﻿using Newtonsoft.Json.Linq;
using System;

namespace SAM 
 // namespace  SAM.Analytical
{
    /// <summary>
    /// Represents an heat humidifier unit unit object in the analytical domain
    /// </summary>
    public class Humidifier : SimpleEquipment, ISection
    {
        public Humidifier()
            : base("Humidifier")
        {

        }

        public Humidifier(string name)
            : base(name)
        {

        }

        public Humidifier(JObject jObject)
            : base(jObject)
        {

        }

        public Humidifier(Humidifier humidifier)
            : base(humidifier)
        {

        }

        public Humidifier(Guid guid, string name)
            : base(guid, name)
        {

        }

        public override bool FromJObject(JObject jObject)
        {
            if (!base.FromJObject(jObject))
                return false;

            // TODO: Implement specific deserialization logic for AirHandlingUnit properties

            return true;
        }

        public override JObject ToJObject()
        {
            JObject jObject = base.ToJObject();
            if (jObject == null)
                return null;

            // TODO: Implement specific serialization logic for AirHandlingUnit properties

            return jObject;
        }
    }
}
