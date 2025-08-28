﻿using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using SAM.Analytical.Grasshopper.Properties;
using SAM.Core.Grasshopper;
using SAM.Geometry.Grasshopper;
using SAM.Geometry.Spatial;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

// using   system.windows.forms;
using SAM.Geometry.Object.Spatial;

namespace   SAM.Analytical.Grasshopper
{
    public class GooPanel : GooJSAMObject<IPanel>, IGH_PreviewData, IGH_BakeAwareData
    {
        public BoundaryType? BoundaryType { get; } = null;

        public bool ShowAll { get; set; } = true;

        public GooPanel()
            : base()
        {
        }

        public GooPanel(IPanel panel)
            : base(panel)
        {
        }

        public GooPanel(IPanel panel, BoundaryType? boundaryType)
            : base(panel)
        {
            BoundaryType = boundaryType;
        }

        public BoundingBox ClippingBox
        {
            get
            {
                if (Value == null)
                    return BoundingBox.Empty;

                return Geometry.Rhino.Convert.ToRhino(Value?.Face3D?.GetBoundingBox());
            }
        }

        public override IGH_Goo Duplicate()
        {
            GooPanel result = new GooPanel(Value, BoundaryType);
            result.ShowAll = ShowAll;

            return result;
        }

        public void DrawViewportWires(GH_PreviewWireArgs args)
        {
            if (Value == null)
            {
                return;
            }

            GooPlanarBoundary3D gooPlanarBoundary3D = null;

            if (Value is ExternalPanel)
            {
                System.Drawing.Color color = Analytical.Query.Color((ExternalPanel)Value);

                gooPlanarBoundary3D = new GooPlanarBoundary3D(new PlanarBoundary3D(Value.Face3D));
                gooPlanarBoundary3D.DrawViewportWires(args, color, color);
                return;
            }

            Panel panel = Value as Panel;
            if (panel == null)
            {
                return;
            }

            System.Drawing.Color color_ExternalEdge = Analytical.Query.Color(panel.PanelType, false);
            System.Drawing.Color color_InternalEdges = Analytical.Query.Color(panel.PanelType, true);

            if (color_ExternalEdge == System.Drawing.Color.Empty)
                color_ExternalEdge = args.Color;

            if (color_InternalEdges == System.Drawing.Color.Empty)
                color_InternalEdges = args.Color;

            gooPlanarBoundary3D = new GooPlanarBoundary3D(panel.PlanarBoundary3D);
            gooPlanarBoundary3D.DrawViewportWires(args, color_ExternalEdge, color_InternalEdges);

            List<Aperture> apertures = panel.Apertures;
            if (apertures != null)
            {
                foreach (Aperture aperture in apertures)
                {
                    if (aperture == null)
                        continue;

                    GooAperture gooAperture = new GooAperture(aperture);
                    gooAperture.DrawViewportWires(args);
                }
            }
        }

        public void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {
            Face3D face3D = Value?.Face3D;
            if (face3D == null)
            {
                return;
            }

            if (!ShowAll)
            {
                Point3D point3D_CameraLocation = Geometry.Rhino.Convert.ToSAM(RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.CameraLocation);
                if (point3D_CameraLocation == null)
                {
                    return;
                }

                double distance = face3D.Distance(point3D_CameraLocation);
                if (distance < 8 || distance > 15)
                {
                    return;
                }
            }

            global::Rhino.Display.DisplayMaterial displayMaterial = BoundaryType != null && BoundaryType.HasValue ? Query.DisplayMaterial(BoundaryType.Value) : Value is Panel ? Query.DisplayMaterial(((Panel)Value).PanelType) : Query.DisplayMaterial(Value as ExternalPanel);
            if (displayMaterial == null)
            {
                displayMaterial = args.Material;
            }

            Brep brep = Geometry.Rhino.Convert.ToRhino_Brep(face3D);
            if (brep == null)
            {
                return;
            }

            args.Pipeline.DrawBrepShaded(brep, displayMaterial);

            if (Value is Panel)
            {
                List<Aperture> apertures = ((Panel)Value).Apertures;
                if (apertures != null)
                {
                    foreach (Aperture aperture in apertures)
                    {
                        foreach (IClosedPlanar3D closedPlanar3D in aperture.GetFace3D().GetEdge3Ds())
                        {
                            global::Rhino.Display.DisplayMaterial displayMaterial_Aperture = Query.DisplayMaterial(aperture.ApertureConstruction.ApertureType);
                            if (displayMaterial_Aperture == null)
                            {
                                displayMaterial_Aperture = args.Material;
                            }

                            GooSAMGeometry gooSAMGeometry_Aperture = new GooSAMGeometry(closedPlanar3D);
                            gooSAMGeometry_Aperture.DrawViewportMeshes(args, displayMaterial_Aperture);
                        }
                    }
                }
            }
        }

        public bool BakeGeometry(RhinoDoc doc, ObjectAttributes att, out Guid obj_guid)
        {
            obj_guid = Guid.Empty;
            if (Value == null)
            {
                return false;
            }

            bool result = Rhino.Modify.BakeGeometry(Value, doc, att, out List<Guid> obj_guids);
            if (!result)
            {
                return false;
            }

            if (obj_guids == null || obj_guids.Count == 0)
            {
                return false;
            }

            if (obj_guids.Count == 1)
            {
                obj_guid = obj_guids[0];
                return result;
            }

            int index = doc.Groups.Add(Value.Guid.ToString());

            Group group = doc.Groups.ElementAt(index);
            foreach (Guid guid in obj_guids)
            {
                doc.Groups.AddToGroup(index, guid);
            }

            obj_guid = group.Id;
            return result;
        }

        public override bool CastFrom(object source)
        {
            if (source is Panel)
            {
                Value = (Panel)source;
                return true;
            }

            if (typeof(IGH_Goo).IsAssignableFrom(source.GetType()))
            {
                object object_Temp = null;

                try
                {
                    object_Temp = (source as dynamic).Value;
                }
                catch
                {
                }

                if (object_Temp is Panel)
                {
                    Value = (Panel)object_Temp;
                    return true;
                }
            }

            return base.CastFrom(source);
        }

        public override bool CastTo<Y>(ref Y target)
        {
            if (Value == null)
                return false;

            if (typeof(Y).IsAssignableFrom(typeof(GH_Mesh)))
            {
                target = (Y)(object)Value.ToGrasshopper_Mesh();
                return true;
            }
            else if (typeof(Y).IsAssignableFrom(typeof(GH_Brep)))
            {
                target = (Y)(object)Value.Face3D?.ToGrasshopper_Brep();
                return true;
            }

            return base.CastTo(ref target);
        }
    }

    public class GooPanelParam : GH_PersistentParam<GooPanel>, IGH_PreviewObject, IGH_BakeAwareObject
    {
        private bool showAll = true;

        public override Guid ComponentGuid => new Guid("278B438C-43EA-4423-999F-B6A906870939");

        // protected    override System.Drawing.Bitmap Icon => Core.Convert.ToBitmap(Resources.SAM_Small);

        bool IGH_PreviewObject.Hidden { get; set; }

        bool IGH_PreviewObject.IsPreviewCapable => !VolatileData.IsEmpty;

        BoundingBox IGH_PreviewObject.ClippingBox => Preview_ComputeClippingBox();

        public bool IsBakeCapable => true;

        void IGH_PreviewObject.DrawViewportMeshes(IGH_PreviewArgs args)
        {
            foreach (var variable in VolatileData.AllData(true))
            {
                GooPanel gooPanel = variable as GooPanel;
                if (gooPanel == null)
                    continue;

                gooPanel.ShowAll = showAll;
            }

            Preview_DrawMeshes(args);
        }

        void IGH_PreviewObject.DrawViewportWires(IGH_PreviewArgs args)
        {
            foreach (var variable in VolatileData.AllData(true))
            {
                GooPanel gooPanel = variable as GooPanel;
                if (gooPanel == null)
                    continue;

                gooPanel.ShowAll = showAll;
            }

            Preview_DrawWires(args);
        }

        public GooPanelParam()
            : base(typeof(Panel).Name, typeof(Panel).Name, typeof(Panel).FullName.Replace(".", " "), "Params", "SAM")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GooPanel> values)
        {
            global::Rhino.Input.Custom.GetObject getObject = new global::Rhino.Input.Custom.GetObject();
            getObject.SetCommandPrompt("Pick Surfaces to create panels");
            getObject.GeometryFilter = ObjectType.Brep;
            getObject.SubObjectSelect = true;
            getObject.DeselectAllBeforePostSelect = false;
            getObject.OneByOnePostSelect = false;
            getObject.GetMultiple(1, 0);

            if (getObject.CommandResult() != Result.Success)
                return GH_GetterResult.cancel;

            if (getObject.ObjectCount == 0)
                return GH_GetterResult.cancel;

            values = new List<GooPanel>();

            for (int i = 0; i < getObject.ObjectCount; i++)
            {
                ObjRef objRef = getObject.Object(i);

                RhinoObject rhinoObject = objRef.Object();
                if (rhinoObject == null)
                    return GH_GetterResult.cancel;

                Brep brep = rhinoObject.Geometry as Brep;
                if (brep == null)
                    return GH_GetterResult.cancel;

                List<ISAMGeometry3D> sAMGeometry3Ds = Geometry.Rhino.Convert.ToSAM(brep);
                if (sAMGeometry3Ds == null)
                {
                    continue;
                }

                List<Panel> panels = Create.Panels(sAMGeometry3Ds);
                if (brep.HasUserData)
                {
                    List<Panel> panels_Old = null;
                    string @string = brep.GetUserString("SAM");
                    if (!string.IsNullOrWhiteSpace(@string))
                    {
                        panels_Old = Core.Convert.ToSAM<Panel>(@string);
                        if (panels_Old != null)
                        {
                            panels_Old.RemoveAll(x => x == null);
                        }

                        if (panels_Old != null && panels_Old.Count != 0)
                        {
                            for (int j = 0; j < panels.Count; j++)
                            {
                                Panel panel_Old = panels_Old.Closest(panels[j].GetInternalPoint3D());
                                if (panel_Old != null)
                                {
                                    panels[j] = Create.Panel(panel_Old.Guid, panel_Old, panels[j].GetFace3D());
                                }
                            }
                        }
                    }
                }

                if (panels == null || panels.Count == 0)
                {
                    continue;
                }

                values.AddRange(panels.FindAll(x => x != null).ConvertAll(x => new GooPanel(x)));
            }

            return GH_GetterResult.success;
        }

        protected override GH_GetterResult Prompt_Singular(ref GooPanel value)
        {
            global::Rhino.Input.Custom.GetObject getObject = new global::Rhino.Input.Custom.GetObject();
            getObject.SetCommandPrompt("Pick Surface to create panel");
            getObject.GeometryFilter = ObjectType.Brep;
            getObject.SubObjectSelect = true;
            getObject.DeselectAllBeforePostSelect = false;
            getObject.OneByOnePostSelect = true;
            getObject.Get();

            if (getObject.CommandResult() != Result.Success)
                return GH_GetterResult.cancel;

            if (getObject.ObjectCount == 0)
                return GH_GetterResult.cancel;

            for (int i = 0; i < getObject.ObjectCount; i++)
            {
                ObjRef objRef = getObject.Object(i);

                RhinoObject rhinoObject = objRef.Object();
                if (rhinoObject == null)
                    return GH_GetterResult.cancel;

                Brep brep = rhinoObject.Geometry as Brep;
                if (brep == null)
                    return GH_GetterResult.cancel;

                List<ISAMGeometry3D> sAMGeometry3Ds = Geometry.Rhino.Convert.ToSAM(brep);
                if (sAMGeometry3Ds == null)
                {
                    continue;
                }

                List<Panel> panels = Create.Panels(sAMGeometry3Ds);
                if (brep.HasUserData)
                {
                    List<Panel> panels_Old = null;
                    string @string = brep.GetUserString("SAM");
                    if (!string.IsNullOrWhiteSpace(@string))
                    {
                        panels_Old = Core.Convert.ToSAM<Panel>(@string);
                        if (panels_Old != null)
                        {
                            panels_Old.RemoveAll(x => x == null);
                        }

                        if (panels_Old != null && panels_Old.Count != 0)
                        {
                            for (int j = 0; j < panels.Count; j++)
                            {
                                Panel panel_Old = panels_Old.Closest(panels[j].GetInternalPoint3D());
                                if (panel_Old != null)
                                {
                                    panels[j] = Create.Panel(panel_Old.Guid, panel_Old, panels[j].GetFace3D());
                                }
                            }
                        }
                    }
                }

                if (panels == null || panels.Count == 0)
                {
                    continue;
                }

                value = new GooPanel(panels[0]);
            }

            return GH_GetterResult.success;
        }

        public void BakeGeometry(RhinoDoc doc, List<Guid> obj_ids)
        {
            BakeGeometry(doc, doc.CreateDefaultAttributes(), obj_ids);
        }

        public void BakeGeometry(RhinoDoc doc, ObjectAttributes att, List<Guid> obj_ids)
        {
            foreach (var value in VolatileData.AllData(true))
            {
                Guid uuid = default;
                (value as IGH_BakeAwareData)?.BakeGeometry(doc, att, out uuid);
                obj_ids.Add(uuid);
            }
        }

        public void BakeGeometry_ByPanelType(RhinoDoc doc)
        {
            Modify.BakeGeometry_ByPanelType(doc, VolatileData, false, Core.Tolerance.Distance);
        }

        private void Menu_BakeByDischargeCoefficient(object sender, EventArgs e)
        {
            Modify.BakeGeometry_ByDischargeCoefficient(RhinoDoc.ActiveDoc, VolatileData);
        }

        public void BakeGeometry_ByConstruction(RhinoDoc doc)
        {
            Modify.BakeGeometry_ByConstruction(doc, VolatileData, false, Core.Tolerance.Distance);
        }

        //public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        //{
        //    Menu_AppendItem(menu, "Show All", Menu_ShowAll, Core.Convert.ToBitmap(Resources.SAM3) , VolatileData.AllData(true).Any(), showAll).Tag = showAll;

        //    Menu_AppendItem(menu, "Bake By Type", Menu_BakeByPanelType, Core.Convert.ToBitmap(Resources.SAM3), VolatileData.AllData(true).Any());
        //    Menu_AppendItem(menu, "Bake By Construction", Menu_BakeByConstruction, Core.Convert.ToBitmap(Resources.SAM3), VolatileData.AllData(true).Any());
        //    Menu_AppendItem(menu, "Bake By Discharge Coefficient", Menu_BakeByDischargeCoefficient, Core.Convert.ToBitmap(Resources.SAM3), VolatileData.AllData(true).Any());
        //    Menu_AppendItem(menu, "Save As...", Menu_SaveAs, Core.Convert.ToBitmap(Resources.SAM3), VolatileData.AllData(true).Any());

        //    if (System.IO.File.Exists(Query.AnalyticalUIPath()))
        //    {
        //        Menu_AppendItem(menu, "Open in UI", Menu_OpenInUI, Core.Convert.ToBitmap(Resources.SAM3), VolatileData.AllData(true).Any());
        //    }

        //    base.AppendAdditionalMenuItems(menu);
        //}

        private void Menu_SaveAs(object sender, EventArgs e)
        {
            Core.Grasshopper.Query.SaveAs(VolatileData);
        }

        private void Menu_BakeByPanelType(object sender, EventArgs e)
        {
            BakeGeometry_ByPanelType(RhinoDoc.ActiveDoc);
        }

        private void Menu_ShowAll(object sender, EventArgs e)
        {
            //if (sender is ToolStripMenuItem item && item.Tag is bool)
            //{
            //    showAll = !(bool)item.Tag;
            //    ExpirePreview(true);
            //}
        }

        private void Menu_BakeByConstruction(object sender, EventArgs e)
        {
            BakeGeometry_ByConstruction(RhinoDoc.ActiveDoc);
        }

        public override bool Write(GH_IWriter writer)
        {
            writer.SetBoolean(GetType().FullName, showAll);

            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            if (reader != null)
                reader.TryGetBoolean(GetType().FullName, ref showAll);

            return base.Read(reader);
        }

        private void Menu_OpenInUI(object sender, EventArgs e)
        {
            Process process = Convert.ToUI(VolatileData);
        }
    }
}