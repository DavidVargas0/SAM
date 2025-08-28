﻿using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using SAM.Analytical.Grasshopper.Properties;
using SAM.Core.Grasshopper;
using System;
using System.Collections.Generic;
using System.Linq;

// // using   system.windows.forms;

namespace   SAM.Analytical.Grasshopper
{
    public class GooApertureConstruction : GooJSAMObject<ApertureConstruction>
    {
        public GooApertureConstruction()
            : base()
        {
        }

        public GooApertureConstruction(ApertureConstruction apertureConstruction)
            : base(apertureConstruction)
        {
        }

        public override IGH_Goo Duplicate()
        {
            return new GooApertureConstruction(Value);
        }

        public override bool CastFrom(object source)
        {
            if (source is ApertureConstruction)
            {
                Value = (ApertureConstruction)source;
                return true;
            }

            if (typeof(IGH_Goo).IsAssignableFrom(source.GetType()))
            {
                try
                {
                    source = (source as dynamic).Value;
                }
                catch
                {
                }

                if (source is ApertureConstruction)
                {
                    Value = (ApertureConstruction)source;
                    return true;
                }
            }

            return base.CastFrom(source);
        }

        public override bool CastTo<Y>(ref Y target)
        {
            return base.CastTo(ref target);
        }
    }

    public class GooApertureConstructionParam : GH_PersistentParam<GooApertureConstruction>
    {
        public override Guid ComponentGuid => new Guid("9b821163-e775-48d9-aee8-dc4d51c4186b");

        //// protected    override System.Drawing.Bitmap Icon => Core.Convert.ToBitmap(Resources.SAM_Small);

        public GooApertureConstructionParam()
            : base(typeof(ApertureConstruction).Name, typeof(ApertureConstruction).Name, typeof(ApertureConstruction).FullName.Replace(".", " "), "Params", "SAM")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GooApertureConstruction> values)
        {
            throw new NotImplementedException();
        }

        protected override GH_GetterResult Prompt_Singular(ref GooApertureConstruction value)
        {
            throw new NotImplementedException();
        }

        //public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        //{
        //    Menu_AppendItem(menu, "Save As...", Menu_SaveAs, Core.Convert.ToBitmap(Resources.SAM3), VolatileData.AllData(true).Any());

        //    //Menu_AppendSeparator(menu);

        //    base.AppendAdditionalMenuItems(menu);
        //}

        private void Menu_SaveAs(object sender, EventArgs e)
        {
            Core.Grasshopper.Query.SaveAs(VolatileData);
        }
    }
}