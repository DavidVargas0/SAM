﻿namespace SAM 
 // namespace  SAM.Geometry.Object.Spatial
{
    public static partial class Query
    {

        public static bool Concave(this IFace3DObject face3Dobject, bool externalEdge = true, bool internalEdges = true)
        {
            return Geometry.Spatial.Query.Concave(face3Dobject?.Face3D, externalEdge, internalEdges);
        }
    }
}