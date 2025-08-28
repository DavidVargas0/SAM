﻿using Newtonsoft.Json.Linq;
// using SAM.Core;
// using SAM.Geometry.Spatial;

namespace SAM 
 // namespace  SAM.Analytical
{
    public class ExternalPanel : SAMInstance<Construction>, IPanel
    {
        private Face3D face3D;

        public ExternalPanel(Face3D face3D, Construction construction)
            : base(construction)
        {
            this.face3D = face3D == null ? null :new Face3D(face3D);
        }

        public ExternalPanel(Face3D face3D)
            : base(null as Construction)
        {
            this.face3D = face3D == null ? null : new Face3D(face3D);
        }

        public ExternalPanel(System.Guid guid, ExternalPanel externalPanel, Face3D face3D)
            : base(externalPanel)
        {
            this.face3D = face3D == null ? null : new Face3D(face3D);
        }

        public ExternalPanel(ExternalPanel externalPanel)
            : base(externalPanel)
        {
            face3D = externalPanel?.Face3D?.Clone<Face3D>();
        }

        public ExternalPanel(JObject jObject)
            : base(jObject)
        {
        }

        public Face3D Face3D
        {
            get
            {
                return face3D == null ? null : new Face3D(face3D);
            }
        }

        public Construction Construction
        {
            get
            {
                return Type;
            }
        }

        public void FlipNormal(bool flipX = true)
        {
            if (face3D == null)
            {
                return;
            }

            face3D.FlipNormal(flipX);
        }

        public override bool FromJObject(JObject jObject)
        {
            if (!base.FromJObject(jObject))
            {
                return false;
            }

            if(jObject.ContainsKey("Face3D"))
            {
                face3D = new Face3D(jObject.Value<JObject>());
            }

            return true;
        }

        public override JObject ToJObject()
        {
           JObject jObject = base.ToJObject();
            if (jObject == null)
            {
                return null;
            }

            if(face3D != null)
            {
                jObject.Add("Face3D", face3D.ToJObject());
            }

            return jObject;
        }
    }
}