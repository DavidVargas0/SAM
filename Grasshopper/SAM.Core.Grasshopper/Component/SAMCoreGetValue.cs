﻿using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using SAM.Core.Grasshopper.Properties;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SAM.Core.Grasshopper
{
    public class SAMCoreGetValue : GH_SAMComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("f111f564-06a8-48bb-8db2-6cf50d07a3f7");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.0";

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        // protected override System.Drawing.Bitmap   Icon => Resources.SAM_Get3;

        private GH_OutputParamManager outputParamManager;

        /// <summary>
        /// Initializes a new instance of the SAM_point3D class.
        /// </summary>
        public SAMCoreGetValue()
          : base("GetValue", "GetValue",
              "Get Value of object property, use 'GetNames' component to find out what can be connected here",
              "SAM", "Core")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager inputParamManager)
        {
            inputParamManager.AddGenericParameter("_object", "_object", "SAM Object", GH_ParamAccess.item);
            int index = inputParamManager.AddTextParameter("_name_", "_name_", "Name", GH_ParamAccess.item, "Name");
            inputParamManager[index].DataMapping = GH_DataMapping.Graft;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager outputParamManager)
        {
            this.outputParamManager = outputParamManager;
            outputParamManager.AddGenericParameter("Value", "Value", "Property Value", GH_ParamAccess.item);
            //outputParamManager.AddGenericParameter("Points", "Pts", "Snap points", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="dataAccess">
        /// The DA object is used to retrieve from inputs and store in outputs.
        /// </param>
        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            string name = null;
            if (!dataAccess.GetData(1, ref name) || string.IsNullOrWhiteSpace(name))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            GH_ObjectWrapper objectWrapper = null;
            if (!dataAccess.GetData(0, ref objectWrapper) || objectWrapper == null)
            {
                dataAccess.SetData(0, null);
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Null object provided");
                return;
            }

            object @object = objectWrapper.Value;

            if (@object is IGH_Goo)
            {
                try
                {
                    @object = (@object as dynamic).Value;
                }
                catch (Exception)
                {
                    @object = null;
                }
            }

            if (@object == null)
            {
                dataAccess.SetData(0, null);
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Null object provided");
                return;
            }

            object value = null;
            if (!Core.Query.TryGetValue(@object, name, out value))
            {
                bool hasValue = false;
                if (@object is SAMObject)
                {
                    List<Enum> enums = ActiveManager.GetParameterEnums(@object.GetType(), name);
                    if (enums != null && enums.Count != 0)
                    {
                        foreach (Enum @enum in enums)
                        {
                            if (((SAMObject)@object).TryGetValue(enums[0], out value))
                            {
                                hasValue = true;
                                break;
                            }
                        }
                    }
                }

                if(!hasValue)
                {
                    if (@object is SAMObject)
                    {
                        SAMObject sAMObject = (SAMObject)@object;
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, string.Format("Property or Method for: {3} not found. {0}:{1} Guid: {2}", sAMObject.GetType().Name, sAMObject.Name, sAMObject.Guid.ToString(), name));
                    }
                    else
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, string.Format("Property or Method {0} not found", name));
                    }

                    return;
                }
            }

            if (value is SAMObject)
            {
                value = new GooJSAMObject<SAMObject>((SAMObject)value);
            }

            if (value is IEnumerable && !(value is string))
                dataAccess.SetDataList(0, (IEnumerable)value);
            else
                dataAccess.SetData(0, value);
        }
    }
}