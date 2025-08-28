﻿using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
// using SAM.Analytical.Grasshopper.Properties;
// using SAM.Core.Grasshopper;
using System;
using System.Collections.Generic;

namespace SAM 
 // namespace   SAM.Analytical.Grasshopper
{
    [Obsolete("Obsolete since 2021.11.24")]
    public class GooRoofType : GooJSAMObject<RoofType>
    {
        public GooRoofType()
            : base()
        {
        }

        public GooRoofType(RoofType roofType)
            : base(roofType)
        {
        }

        public override IGH_Goo Duplicate()
        {
            return new GooRoofType(Value);
        }
    }

    [Obsolete("Obsolete since 2021.11.24")]
    public class GooRoofTypeParam : GH_PersistentParam<GooRoofType>
    {
        public override Guid ComponentGuid => new Guid("4986fedf-7b51-4873-aef6-b6768412b9e9");

                // protected    override System.Drawing.Bitmap Icon => Core.Convert.ToBitmap(Resources.SAM_Small);

        public override GH_Exposure Exposure => GH_Exposure.hidden;

        public GooRoofTypeParam()
            : base(typeof(RoofType).Name, typeof(RoofType).Name, typeof(RoofType).FullName.Replace(".", " "), "Params", "SAM")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GooRoofType> values)
        {
            throw new NotImplementedException();
        }

        protected override GH_GetterResult Prompt_Singular(ref GooRoofType value)
        {
            throw new NotImplementedException();
        }
    }
}