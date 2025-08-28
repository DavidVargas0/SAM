﻿using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using SAM.Analytical.Grasshopper.Properties;
using SAM.Core.Grasshopper;
using System;
using System.Collections.Generic;

namespace   SAM.Analytical.Grasshopper
{
    [Obsolete("Obsolete since 2021.11.24")]
    public class GooHostPartitionType : GooJSAMObject<HostPartitionType>
    {
        public GooHostPartitionType()
            : base()
        {
        }

        public GooHostPartitionType(HostPartitionType hostPartitionType)
            : base(hostPartitionType)
        {
        }

        public override IGH_Goo Duplicate()
        {
            return new GooHostPartitionType(Value);
        }
    }

    [Obsolete("Obsolete since 2021.11.24")]
    public class GooHostPartitionTypeParam : GH_PersistentParam<GooHostPartitionType>
    {
        public override Guid ComponentGuid => new Guid("d906080a-02e8-4a53-b394-ab9889a8b79f");

                // protected    override System.Drawing.Bitmap Icon => Core.Convert.ToBitmap(Resources.SAM_Small);

        public override GH_Exposure Exposure => GH_Exposure.hidden;

        public GooHostPartitionTypeParam()
            : base(typeof(HostPartitionType).Name, typeof(HostPartitionType).Name, typeof(HostPartitionType).FullName.Replace(".", " "), "Params", "SAM")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GooHostPartitionType> values)
        {
            throw new NotImplementedException();
        }

        protected override GH_GetterResult Prompt_Singular(ref GooHostPartitionType value)
        {
            throw new NotImplementedException();
        }
    }
}