﻿// using SAM.Geometry.Object.Spatial;
using System;

namespace SAM 
 // namespace  SAM.Analytical
{
    public interface IPanel : IFace3DObject, IAnalyticalObject
    {
        Guid Guid { get; }
    }
}
