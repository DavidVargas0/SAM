﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAM 
 // namespace  SAM.Geometry.Spatial
{
    public class Mesh3D : SAMGeometry, IMesh, ISAMGeometry3D, IBoundable3D
    {
        private List<Point3D> points;
        private List<Tuple<int, int, int>> indexes;
        
        public Mesh3D(JObject jObject)
        {
            FromJObject(jObject);
        }

        public Mesh3D(Mesh3D mesh3D)
        {
            points = mesh3D?.points?.ConvertAll(x => new Point3D(x));
            indexes = mesh3D?.indexes?.ConvertAll(x => new Tuple<int, int, int>(x.Item1, x.Item2, x.Item3));
        }

        public Mesh3D(IEnumerable<Triangle3D> triangle3Ds)
        {
            if(triangle3Ds != null)
            {
                points = new List<Point3D>();
                indexes = new List<Tuple<int, int, int>>();
                foreach (Triangle3D triangle3D in triangle3Ds)
                {
                    if(triangle3D == null)
                    {
                        continue;
                    }

                    int[] array = new int[3];
                    for (int i=0; i < 3; i++)
                    {
                        Point3D point3D = triangle3D[i];

                        int index = points.FindIndex(x => x.Equals(point3D));
                        if(index == -1)
                        {
                            index = points.Count;
                            points.Add(point3D);
                        }
                        array[i] = index;
                    }

                    indexes.Add(new Tuple<int, int, int>(array[0], array[1], array[2]));
                }
            }
        }

        public Mesh3D(IEnumerable<Point3D> points, IEnumerable<Tuple<int, int, int>> indexes)
        {
            this.points = points?.ToList().ConvertAll(x => new Point3D(x));
            this.indexes = indexes?.ToList().ConvertAll(x => new Tuple<int, int, int>(x.Item1, x.Item2, x.Item3));
        }

        public ISAMGeometry3D GetMoved(Vector3D vector3D)
        {
            return new Mesh3D(points?.ConvertAll(x => x.GetMoved(vector3D) as Point3D), indexes);
        }

        public int IndexOf(Point3D point3D)
        {
            if (point3D == null || points == null)
            {
                return -1;
            }

            for (int i = 0; i < points.Count; i++)
            {
                if (point3D.Equals(points[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public int IndexOf(Point3D point3D, double tolerance)
        {
            if (point3D == null || points == null)
            {
                return -1;
            }

            for (int i = 0; i < points.Count; i++)
            {
                if (point3D.AlmostEquals(points[i], tolerance))
                {
                    return i;
                }
            }

            return -1;
        }

        public ISAMGeometry3D GetTransformed(Transform3D transform3D)
        {
            return new Mesh3D(points?.ConvertAll(x => x.GetTransformed(transform3D) as Point3D), indexes);
        }

        public override bool FromJObject(JObject jObject)
        {
            if (jObject.ContainsKey("Points"))
                points = Create.Point3Ds(jObject.Value<JArray>("Points"));

            if (jObject.ContainsKey("Indexes"))
            {
                indexes = new List<Tuple<int, int, int>>();

                JArray jArray = jObject.Value<JArray>("Indexes");
                if(jArray != null)
                {
                    foreach (JArray jArray_Temp in jArray)
                    {
                        if(jArray_Temp == null || jArray_Temp.Count < 3)
                        {
                            continue;
                        }

                        indexes.Add(new Tuple<int, int, int>(jArray_Temp[0].Value<int>(), jArray_Temp[1].Value<int>(), jArray_Temp[2].Value<int>()));
                    }
                }
            }

            return true;
        }

        public override JObject ToJObject()
        {
            JObject jObject = base.ToJObject();
            if (jObject == null)
                return null;

            if (points != null)
                jObject.Add("Points", Geometry.Create.JArray(points));

            if(indexes != null)
            {
                JArray jArray = new JArray();

                foreach(Tuple<int, int, int> tuple in indexes)
                {
                    JArray jArray_Temp = new JArray();
                    jArray_Temp.Add(tuple.Item1);
                    jArray_Temp.Add(tuple.Item2);
                    jArray_Temp.Add(tuple.Item3);
                    jArray.Add(jArray_Temp);
                }

                jObject.Add("Indexes", jArray);
            }

            return jObject;
        }

        public override ISAMGeometry Clone()
        {
            return new Mesh3D(points, indexes);
        }

        public int TrianglesCount
        {
            get
            {
                if (indexes == null)
                {
                    return -1;
                }

                return indexes.Count;

            }
        }

        public int PointsCount
        {
            get
            {
                if (points == null)
                {
                    return -1;
                }

                return points.Count;
            }
        }

        public HashSet<int> ConnectedIndexes(int index)
        {
            if(indexes == null || indexes.Count == 0)
            {
                return null;
            }

            if(index < 0 || index >= points.Count)
            {
                return null;
            }

            HashSet<int> result = new HashSet<int>();
            foreach(Tuple<int, int, int> tuple in indexes)
            {
                if(index == tuple.Item1)
                {
                    result.Add(tuple.Item2);
                    result.Add(tuple.Item3);
                }
                else if(index == tuple.Item2)
                {
                    result.Add(tuple.Item1);
                    result.Add(tuple.Item3);
                }
                else if (index == tuple.Item3)
                {
                    result.Add(tuple.Item1);
                    result.Add(tuple.Item2);
                }
            }

            return result;
        }

        public HashSet<int> ConnectedIndexes(Point3D point3D)
        {
            int index = IndexOf(point3D);
            if(index == -1)
            {
                return null;
            }

            return ConnectedIndexes(index);
        }

        public HashSet<int> ConnectedIndexes(Point3D point3D, double tolerance)
        {
            int index = IndexOf(point3D, tolerance);
            if (index == -1)
            {
                return null;
            }

            return ConnectedIndexes(index);
        }

        public HashSet<Point3D> ConnectedPoint3Ds(int index)
        {
            HashSet<int> indexes = ConnectedIndexes(index);
            if(indexes == null)
            {
                return null;
            }

            HashSet<Point3D> result = new HashSet<Point3D>();
            foreach(int index_Connected in indexes)
            {
                result.Add(points[index_Connected]);
            }

            return result;
        }

        public HashSet<Point3D> ConnectedPoint3Ds(Point3D point3D)
        {
            int index = IndexOf(point3D);
            if (index == -1)
            {
                return null;
            }

            return ConnectedPoint3Ds(index);
        }

        public HashSet<Point3D> ConnectedPoint3Ds(Point3D point3D, double tolerance)
        {
            int index = IndexOf(point3D, tolerance);
            if (index == -1)
            {
                return null;
            }

            return ConnectedPoint3Ds(index);
        }

        public Triangle3D GetTriangle(int index)
        {
            Tuple<int, int, int> tuple = GetTriangleIndexes(index);
            if(tuple == null)
            {
                return null;
            }

            return new Triangle3D(points[tuple.Item1], points[tuple.Item2], points[tuple.Item3]);
        }

        public Tuple<int, int, int> GetTriangleIndexes(int index)
        {
            if (points == null || indexes == null)
            {
                return null;
            }

            if (index < 0 || index >= indexes.Count)
            {
                return null;
            }

            int index_1 = indexes[index].Item1;
            if (index_1 < 0 || index_1 >= points.Count)
            {
                return null;
            }

            int index_2 = indexes[index].Item2;
            if (index_2 < 0 || index_2 >= points.Count)
            {
                return null;
            }

            int index_3 = indexes[index].Item3;
            if (index_3 < 0 || index_3 >= points.Count)
            {
                return null;
            }

            return new Tuple<int, int, int>(index_1, index_2, index_3);
        }

        public Vector3D GetNormal(int index)
        {
            Tuple<int, int, int> triangleIndexes = GetTriangleIndexes(index);
            if(triangleIndexes == null)
            {
                return null;
            }

            return Query.Normal(points[triangleIndexes.Item1], points[triangleIndexes.Item2], points[triangleIndexes.Item3]);
        }

        public List<int> GetTriangleIndexes(int index_1, int index_2, int maxCount = int.MaxValue)
        {
            if(points == null || points.Count < 3 || indexes == null || indexes.Count == 0)
            {
                return null;
            }

            if (index_1 == index_2)
            {
                return null;
            }

            if (index_1 < 0 || index_1 >= points.Count || index_2 < 0 || index_2 >= points.Count)
            {
                return null;
            }

            List<int> result = new List<int>();
            for(int i=0; i < indexes.Count; i++)
            {
                Tuple<int, int, int> tuple = indexes[i];
                if(tuple == null)
                {
                    continue;
                }

                if(tuple.Item1 != index_1 && tuple.Item2 != index_1 && tuple.Item3 != index_1)
                {
                    continue;
                }

                if (tuple.Item1 != index_2 && tuple.Item2 != index_2 && tuple.Item3 != index_2)
                {
                    continue;
                }

                result.Add(i);

                if(result.Count >= maxCount)
                {
                    break;
                }
            }

            return result;
        }

        public List<Triangle3D> GetTriangles()
        {
            if(points == null || indexes == null)
            {
                return null;
            }

            int count = TrianglesCount;
            if (count == -1)
            {
                return null;
            }

            List<Triangle3D> result = new List<Triangle3D>();
            if(count == 0)
            {
                return result;
            }
            
            for (int i=0; i < TrianglesCount; i++)
            {
                result.Add(GetTriangle(i));
            }

            return result;
        }

        public List<Segment3D> GetSegments()
        {
            return GetSegments(false);
        }

        public List<Segment3D> GetSegments(bool includeSimiliar)
        {
            if(points == null || indexes == null)
            {
                return null;
            }

            List<Segment3D> result = new List<Segment3D>();

            if (includeSimiliar)
            {
                List<Triangle3D> triangle3Ds = GetTriangles();
                if(triangle3Ds == null)
                {
                    return null;
                }

                foreach(Triangle3D triangle3D in triangle3Ds)
                {
                    List<Segment3D> segment3Ds_Triangle3D = triangle3D?.GetSegments();
                    if(segment3Ds_Triangle3D != null && segment3Ds_Triangle3D.Count != 0)
                    {
                        result.AddRange(segment3Ds_Triangle3D);
                    }
                }

                return result;
            }
            else
            {
                List<Tuple<int, int>> tuples = new List<Tuple<int, int>>();

                foreach (Tuple<int, int, int> tuple in indexes)
                {
                    List<int> indexes_Triangle = new List<int>() { tuple.Item1, tuple.Item2, tuple.Item3 };
                    indexes_Triangle.Sort();

                    int index = -1;

                    index = tuples.FindIndex(x => x.Item1 == indexes_Triangle[0] && x.Item2 == indexes_Triangle[1]);
                    if (index == -1)
                    {
                        tuples.Add(new Tuple<int, int>(indexes_Triangle[0], indexes_Triangle[1]));
                    }

                    index = tuples.FindIndex(x => x.Item1 == indexes_Triangle[0] && x.Item2 == indexes_Triangle[2]);
                    if (index == -1)
                    {
                        tuples.Add(new Tuple<int, int>(indexes_Triangle[0], indexes_Triangle[2]));
                    }

                    index = tuples.FindIndex(x => x.Item1 == indexes_Triangle[1] && x.Item2 == indexes_Triangle[2]);
                    if (index == -1)
                    {
                        tuples.Add(new Tuple<int, int>(indexes_Triangle[1], indexes_Triangle[2]));
                    }
                }

                foreach (Tuple<int, int> tuple in tuples)
                {
                    if (tuple.Item1 < 0 || tuple.Item1 >= points.Count)
                    {
                        continue;
                    }

                    if (tuple.Item2 < 0 || tuple.Item2 >= points.Count)
                    {
                        continue;
                    }

                    result.Add(new Segment3D(points[tuple.Item1], points[tuple.Item2]));
                }
            }

            return result;
        }

        public List<Point3D> GetPoints()
        {
            return points?.ConvertAll(x => new Point3D(x));
        }

        public bool On(Point3D point3D, double tolerance = Core.Tolerance.Distance)
        {
            if(points == null || indexes == null)
            {
                return false;
            }
            
            if(!GetBoundingBox().InRange(point3D, tolerance))
            {
                return false;
            }

            for(int i=0; i < indexes.Count; i++)
            {
                Triangle3D triangle3D = GetTriangle(i);
                if(triangle3D == null)
                {
                    continue;
                }

                if(!triangle3D.GetBoundingBox().InRange(point3D, tolerance))
                {
                    continue;
                }

                double distance = new Face3D(triangle3D).Distance(point3D, tolerance);
                if(distance < tolerance)
                {
                    return true;
                }
            }

            return false;
        }

        public bool OnEdge(Point3D point3D, double tolerance = Core.Tolerance.Distance)
        {
            if (points == null)
            {
                return false;
            }

            if (!GetBoundingBox().InRange(point3D, tolerance))
            {
                return false;
            }

            List<Segment3D> segment3Ds = GetSegments(false);
            if (segment3Ds == null || segment3Ds.Count == 0)
            {
                return false;
            }

            return segment3Ds.Find(x => x.On(point3D, tolerance)) != null;
        }

        public List<ICurve3D> GetCurves()
        {
            return GetSegments()?.ConvertAll(x => x as ICurve3D);
        }

        public BoundingBox3D GetBoundingBox(double offset = 0)
        {
            if (points == null)
            {
                return null;
            }

            return new BoundingBox3D(points, offset);
        }
    }
}
