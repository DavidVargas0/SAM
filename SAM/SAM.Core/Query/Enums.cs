﻿using SAM.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace  SAM.Core
{
    public static partial class Query
    {
        public static List<Enum> Enums(SAMObject sAMObject, bool notPublic = false)
        {
            Type type = sAMObject?.GetType();
            if(type == null)
            {
                return null;
            }

            List<Enum> enums = Enums(type, notPublic);
            if(enums == null || enums.Count == 0)
            {
                return null;
            }

            List<Enum> result = new ();
            foreach (Enum @enum in enums)
            {
                if(sAMObject.HasParameter(@enum))
                {
                    result.Add(@enum);
                }
            }

            return result;
        }

        public static List<Enum> Enums(Type type, string value, bool notPublic = false)
        {
            if (type == null || string.IsNullOrEmpty(value))
                return null;

            Dictionary<Type, AssociatedTypes> dictionary = AssociatedTypesDictionary(null, true, notPublic);
            if (dictionary == null)
                return null;

            List<Enum> result = new ();
            foreach (KeyValuePair<Type, AssociatedTypes> keyValuePair in dictionary)
            {
                if (!keyValuePair.Value.IsValid(type))
                    continue;

                foreach (Enum @enum in System.Enum.GetValues(keyValuePair.Key))
                {
                    if (@enum.ToString().Equals(value))
                    {
                        result.Add(@enum);
                        continue;
                    }

                    ParameterProperties parameterProperties = ParameterProperties.Get(@enum);
                    if (parameterProperties == null)
                        continue;

                    string name = parameterProperties.Name;
                    if (string.IsNullOrEmpty(name))
                        continue;

                    if (name.Equals(value))
                        result.Add(@enum);
                }
            }

            return result;
        }

        public static List<Enum> Enums(Type type, bool notPublic = false)
        {
            if (type == null)
                return null;

            Dictionary<Type, AssociatedTypes> dictionary = AssociatedTypesDictionary(null, true, notPublic);
            if (dictionary == null)
                return null;

            List<Enum> result = new ();
            foreach (KeyValuePair<Type, AssociatedTypes> keyValuePair in dictionary)
            {
                if (!keyValuePair.Value.IsValid(type))
                    continue;

                foreach (Enum @enum in System.Enum.GetValues(keyValuePair.Key))
                {
                    ParameterProperties parameterProperties = ParameterProperties.Get(@enum);
                    if (parameterProperties == null)
                        continue;

                    result.Add(@enum);
                }
            }

            return result;
        }

        public static List<Enum> Enums(params Type[] types)
        {
            if (types == null)
                return null;

            List<Enum> result = new ();
            foreach(Type type in types)
            {
                if (type == null )
                    continue;

                if(type.IsEnum)
                {
                    foreach (Enum @enum in System.Enum.GetValues(type))
                    {
                        result.Add(@enum);
                    }
                }
                else
                {
                    List<Enum> enums = Enums(type, false);
                    if(enums != null)
                    {
                        result.AddRange(enums);
                    }
                }

            }

            return result;
        }

        public static List<T> Enums<T>(params T[] excluded) where T : Enum
        {
            List<T> result = new List<T>();

            Array array = System.Enum.GetValues(typeof(T));

            for (int i = 0; i < array.Length; i++)
            {
                T value = (T)array.GetValue(i);

                if(excluded != null && excluded.Contains(value))
                {
                    continue;
                }

                result.Add(value);
            }

            return result;
        }
    }
}