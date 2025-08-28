using System;

namespace SAM 
 // namespace  SAM.Core
{
    public interface ISAMObject : IJSAMObject
    {
        Guid Guid { get; }
        string Name { get; }
    }
}