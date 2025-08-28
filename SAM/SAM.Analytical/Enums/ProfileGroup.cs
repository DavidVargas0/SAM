﻿using System.ComponentModel;

namespace SAM 
 // namespace  SAM.Analytical
{
    [Description("Analytical Profile Group.")]
    public enum ProfileGroup
    {
        [Description("Undefined")] Undefined,
        [Description("Gain")] Gain,
        [Description("Thermostat")] Thermostat,
        [Description("Humidistat")] Humidistat,
        [Description("Ventilation")] Ventilation,
    }
}