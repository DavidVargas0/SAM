﻿using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Newtonsoft.Json.Linq;
using SAM.Math.Grasshopper.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Windows.Forms;

namespace   SAM.Math.Grasshopper
{
    public class GooMatrix : GH_Goo<Matrix>
    {
        public GooMatrix()
            : base()
        {
        }

        public GooMatrix(Matrix matrix)
        {
            Value = matrix;
        }

        public override bool IsValid => Value != null;

        public override string TypeName
        {
            get
            {
                if (Value == null)
                    return typeof(Matrix).Name;
                else
                    return Value.GetType().Name;
            }
        }

        public override string TypeDescription
        {
            get
            {
                if (Value == null)
                    return typeof(Matrix).Name;
                else
                    return Value.GetType().Name;
            }
        }

        public override IGH_Goo Duplicate()
        {
            return new GooMatrix(Value.Clone());
        }

        public override bool Write(GH_IWriter writer)
        {
            if (Value == null)
                return false;

            JObject jObject = Value.ToJObject();
            if (jObject == null)
                return false;

            writer.SetString(typeof(Matrix).FullName, jObject.ToString());
            return true;
        }

        public override bool Read(GH_IReader reader)
        {
            string value = null;
            if (!reader.TryGetString(typeof(Matrix).FullName, ref value))
                return false;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            Value = Create.Matrix(JObject.Parse(value));
            return true;
        }

        public override string ToString()
        {
            if (Value == null)
                return typeof(Matrix).Name;

            return Value?.GetType().Name;
        }

        public override bool CastFrom(object source)
        {
            if (source is Matrix)
            {
                Value = (Matrix)source;
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

                if (source is Matrix)
                {
                    Value = (Matrix)source;
                    return true;
                }
            }

            if (source is GH_Matrix)
            {
                Value = Convert.ToSAM(((GH_Matrix)source));
                return true;
            }

            if (source is Matrix)
            {
                Value = Rhino.Convert.ToSAM(((global::Rhino.Geometry.Matrix)source));
                return true;
            }

            return base.CastFrom(source);
        }

        public override bool CastTo<Y>(ref Y target)
        {
            if (typeof(Y) == typeof(Matrix))
            {
                target = (Y)(object)Value;
                return true;
            }

            if (typeof(Y) == typeof(GH_Matrix))
            {
                target = (Y)(object)Value.ToGrasshopper();
            }

            if (typeof(Y) == typeof(global::Rhino.Geometry.Matrix))
            {
                target = (Y)(object)Rhino.Convert.ToRhino(Value);
            }

            if (typeof(Y).IsAssignableFrom(Value.GetType()))
            {
                target = (Y)(object)Value;
                return true;
            }

            return base.CastTo(ref target);
        }
    }

    public class GooMatrixParam : GH_PersistentParam<GooMatrix>
    {
        public override Guid ComponentGuid => new Guid("79fbe851-272a-4c4e-8d0a-6033eed12102");

        //protected override System.Drawing.Bitmap Icon => Resources.SAM_Small;

        public override GH_Exposure Exposure => GH_Exposure.hidden;

        public bool IsBakeCapable => false;

        public GooMatrixParam()
            : base(typeof(Matrix).Name, typeof(Matrix).Name, typeof(Matrix).FullName.Replace(".", " "), "Params", "SAM")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GooMatrix> values)
        {
            throw new NotImplementedException();
        }

        protected override GH_GetterResult Prompt_Singular(ref GooMatrix value)
        {
            throw new NotImplementedException();
        }

        //public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        //{
        //    Menu_AppendItem(menu, "Save As...", Menu_SaveAs, VolatileData.AllData(true).Any());

        //    //Menu_AppendSeparator(menu);

        //    base.AppendAdditionalMenuItems(menu);
        //}

        //private void Menu_SaveAs(object sender, EventArgs e)
        //{
        //    Core.Grasshopper.Query.SaveAs(VolatileData);
        //}
    }
}