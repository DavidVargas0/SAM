﻿using System;

namespace SAM.Units
//
// namespace SAM.Units
//.Units
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class Abbreviation : Attribute
    {
        private string value;

        public Abbreviation(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get
            {
                return value;
            }
        }
    }
}