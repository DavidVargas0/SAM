﻿using System.Collections.Generic;
using System.Linq;

namespace SAM 
 // namespace  SAM.Core
{
    public static partial class Query
    {
        public static T Max<T>(this IEnumerable<Range<T>> ranges)
        {
            if(ranges == null)
            {
                return default;
            }

            List<T> values = new List<T>();
            foreach(Range<T> range in ranges)
            {
                values.Add(range.Max);
            }

            if(values.Count == 0)
            {
                return default;
            }

            return values.Max();
        }
    }
}