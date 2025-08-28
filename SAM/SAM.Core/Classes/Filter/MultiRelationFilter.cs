﻿using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SAM 
 // namespace  SAM.Core
{
    public abstract class MultiRelationFilter<T> : Filter, IMultiRelationFilter where T : IJSAMObject
    {
        public MultiRelationFilter(JObject jObject)
            : base(jObject)
        {
        }

        public MultiRelationFilter()
            : base()
        {
        }

        public MultiRelationFilter(MultiRelationFilter<T> multiRelationFilter)
            : base(multiRelationFilter)
        {
            if (multiRelationFilter != null)
            {
                FilterLogicalOperator = multiRelationFilter.FilterLogicalOperator;
                Filter = multiRelationFilter.Filter?.Clone();
            }
        }

        public IFilter Filter { get; set; }

        public FilterLogicalOperator FilterLogicalOperator { get; set; } = FilterLogicalOperator.Or;

        public override bool FromJObject(JObject jObject)
        {
            if (!base.FromJObject(jObject))
            {
                return false;
            }

            if (jObject.ContainsKey("FilterLogicalOperator"))
            {
                FilterLogicalOperator = Query.Enum<FilterLogicalOperator>(jObject.Value<string>("FilterLogicalOperator"));
            }

            if (jObject.ContainsKey("Filter"))
            {
                Filter = Query.IJSAMObject(jObject.Value<JObject>("Filter")) as IFilter;
            }

            return true;
        }

        public abstract List<T> GetRelatives(IJSAMObject jSAMObject);

        public override bool IsValid(IJSAMObject jSAMObject)
        {
            if (jSAMObject == null || FilterLogicalOperator == FilterLogicalOperator.Undefined)
            {
                return false;
            }

            if (Filter == null)
            {
                return true;
            }

            List<T> relatives = GetRelatives(jSAMObject);
            if (relatives == null || relatives.Count == 0)
            {
                return Filter.Inverted ? true : false;
            }

            bool result = false;
            if (FilterLogicalOperator == FilterLogicalOperator.And)
            {
                result = relatives.TrueForAll(x => Filter.IsValid(x));
            }
            else if (FilterLogicalOperator == FilterLogicalOperator.Or)
            {
                result = relatives.Find(x => Filter.IsValid(x)) != null;
            }

            if (Inverted)
            {
                result = !result;
            }

            return result;
        }

        public override JObject ToJObject()
        {
            JObject result = base.ToJObject();
            if (result == null)
            {
                return result;
            }

            if (Filter != null)
            {
                result.Add("Filter", Filter.ToJObject());
            }

            result.Add("FilterLogicalOperator", FilterLogicalOperator.ToString());

            return result;
        }
    }
}