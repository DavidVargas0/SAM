using System.Collections;
using System.Collections.Generic;

namespace SAM 
 // namespace  SAM.Core
{
    public interface IIndexedObjects : IJSAMObject, IEnumerable
    {
    }

    public interface IIndexedObjects<T> : IIndexedObjects, IEnumerable<T>
    {
    }
}