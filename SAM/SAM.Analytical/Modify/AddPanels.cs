﻿// using SAM.Geometry.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAM 
 // namespace  SAM.Analytical
{
    public static partial class Modify
    {
        public static List<Panel> AddPanels(this AdjacencyCluster adjacencyCluster, IEnumerable<Face3D> face3Ds, Construction construction, IEnumerable<Space> spaces = null, double silverSpacing = Tolerance.MacroDistance, double tolerance_Angle = Tolerance.Angle, double tolerance_Distance = Tolerance.Distance, double tolerance_Snap = Tolerance.MacroDistance)
        {
            if(adjacencyCluster == null || face3Ds == null)
            {
                return null;
            }

            List<Space> spaces_Temp = spaces == null ? adjacencyCluster.GetSpaces() : new List<Space>(spaces);
            if(spaces_Temp == null || spaces_Temp.Count == 0)
            {
                return null;
            }

            List<Tuple<Face3D, BoundingBox3D>> tuples = face3Ds.ToList().FindAll(x => x != null).ConvertAll(x => new Tuple<Face3D, BoundingBox3D>(x, x.GetBoundingBox()));
            BoundingBox3D boundingBox3D = new BoundingBox3D(tuples.ConvertAll(x => x.Item2));


            List<Panel> result = new List<Panel>();
            foreach(Space space in spaces_Temp)
            {
                Shell shell = adjacencyCluster.Shell(space);

                BoundingBox3D boundingBox3D_Shell = shell?.GetBoundingBox();
                if(boundingBox3D_Shell == null)
                {
                    continue;
                }

                if(!boundingBox3D.InRange(boundingBox3D_Shell, tolerance_Snap))
                {
                    continue;
                }

                List<Face3D> face3Ds_Temp = tuples.FindAll(x => x.Item2.InRange(boundingBox3D_Shell, tolerance_Snap))?.ConvertAll(x => x.Item1);
                if(face3Ds_Temp == null || face3Ds_Temp.Count == 0)
                {
                    continue;
                }

                List<Shell> shells_Split = Geometry.Spatial.Query.Split(shell, face3Ds_Temp, tolerance_Snap, tolerance_Angle, tolerance_Distance);
                if(shells_Split == null || shells_Split.Count < 2)
                {
                    continue;
                }

                List<Panel> panels = adjacencyCluster.GetPanels(space);

                PanelType panelType = PanelType.Air;
                if(construction != null)
                {
                    panelType = construction.PanelType();
                }

                if(panelType == PanelType.Undefined)
                {
                    panelType = PanelType.Air;
                }
                else if(panelType == PanelType.Wall)
                {
                    panelType = PanelType.WallInternal;
                }
                else if(panelType == PanelType.Floor)
                {
                    panelType = PanelType.FloorInternal;
                }

                List<Panel> panels_New = new List<Panel>();
                for(int i = 0; i < shells_Split.Count; i++)
                {
                    Shell shell_Split = shells_Split[i];

                    Point3D point3D = shell_Split.CalculatedInternalPoint3D(tolerance_Snap, tolerance_Distance);
                    if (point3D == null)
                    {
                        continue;
                    }
                    string name = space.Name;
                    if(name != null)
                    {
                        name += string.Format("_{0}", i + 1);
                    }

                    Space space_Shell = new Space(Guid.NewGuid(), space, name, point3D);

                    adjacencyCluster.AddObject(space_Shell);

                    List<Face3D> face3Ds_Shell = shell_Split.Face3Ds;
                    foreach(Face3D face3D_Shell in face3Ds_Shell)
                    {
                        Point3D point3D_Face3D = face3D_Shell?.GetInternalPoint3D(tolerance_Distance);
                        if(point3D_Face3D == null)
                        {
                            continue;
                        }

                        Panel panel = panels.Find(x => x.Face3D.On(point3D_Face3D, tolerance_Snap));
                        List<Space> spaces_Panel = null;
                        if (panel == null)
                        {
                            panel = panels_New.Find(x => x.Face3D.On(point3D_Face3D, tolerance_Snap));
                            if (panel == null)
                            {
                                panel = Create.Panel(construction, panelType, face3D_Shell);
                                panels_New.Add(panel);
                            }
                        }
                        else
                        {
                            spaces_Panel = adjacencyCluster.GetSpaces(panel);
                            spaces_Panel?.RemoveAll(x => x != null && x.Guid == space.Guid);

                            //panel.Apertures?.ConvertAll(x => new Aperture(x))

                            panel = Create.Panel(Guid.NewGuid(), panel, face3D_Shell, null, true);
                        }

                        if(spaces_Panel == null)
                        {
                            spaces_Panel = new List<Space>();
                        }

                        spaces_Panel.Add(space_Shell);

                        List<Aperture> apertures = panel.Apertures;
                        if (apertures != null && apertures.Count != 0)
                        {
                            for (int j = 0; j < apertures.Count; j++)
                            {
                                Aperture aperture = apertures[j];
                                if (aperture == null)
                                {
                                    continue;
                                }

                                Guid guid = aperture.Guid;

                                Panel panel_Temp = adjacencyCluster.GetPanel(aperture);
                                while (panel_Temp != null && panel_Temp.Guid != panel.Guid)
                                {
                                    aperture = new Aperture(Guid.NewGuid(), aperture);
                                    panel_Temp = adjacencyCluster.GetPanel(aperture);
                                }

                                if(guid == aperture.Guid)
                                {
                                    continue;
                                }

                                panel.RemoveAperture(guid);
                                panel.AddAperture(aperture);
                            }
                        }

                        adjacencyCluster.AddObject(panel);

                        foreach(Space space_Panel in spaces_Panel)
                        {
                            adjacencyCluster.AddRelation(space_Panel, panel);
                        }
                    }

                    adjacencyCluster.UpdateAreaAndVolume(space_Shell);
                }

                result.AddRange(panels_New);

                foreach(Panel panel in panels)
                {
                    adjacencyCluster.RemoveObject<Panel>(panel.Guid);
                }

                adjacencyCluster.RemoveObject<Space>(space.Guid);
            }

            return result;
        }
    }
}