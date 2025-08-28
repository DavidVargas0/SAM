﻿using Newtonsoft.Json.Linq;
using SAM.Core;
using System;

namespace  SAM.Analytical
{
    /// <summary>
    /// Represents an simple equipment object in the analytical domain
    /// </summary>
    public abstract class SimpleEquipment : SAMObject, ISimpleEquipment
    {
        public SimpleEquipment(string name)
            : base(name)
        {

        }

        public SimpleEquipment(JObject jObject)
            : base(jObject)
        {

        }

        public SimpleEquipment(SimpleEquipment simpleEquipment)
            : base(simpleEquipment)
        {

        }

        public SimpleEquipment(Guid guid, string name)
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
