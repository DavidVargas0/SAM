﻿using System.Reflection;

namespace SAM 
 // namespace  SAM.Core
{
    public static partial class Query
    {
        public static bool TryInvokeRuntimeMethod<T>(object @object, string methodName, out T result, params object[] parameters)
        {
            return TryInvokeMethod(@object, @object?.GetType().GetRuntimeMethods(), methodName, out result, parameters);
        }
    }
}