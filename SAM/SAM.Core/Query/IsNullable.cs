using System;

namespace SAM 
 // namespace  SAM.Core
{
    public static partial class Query
    {
        public static bool IsNullable(this Type type)
        {
            if (type == null)
                return false;

            return Nullable.GetUnderlyingType(type) != null;
        }
    }
}