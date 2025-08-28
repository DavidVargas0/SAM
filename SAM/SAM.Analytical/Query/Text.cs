using System;

namespace SAM 
 // namespace  SAM.Analytical
{
    public static partial class Query
    {
        public static string Text(this Enum @enum)
        {
            return Core.Query.Description(@enum);
        }
    }
}