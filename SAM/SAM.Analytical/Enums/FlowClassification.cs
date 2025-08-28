using System.ComponentModel;

namespace SAM 
 // namespace  SAM.Analytical
{
    [Description("Flow Classification")]
    public enum FlowClassification
    {
        [Description("Undefined")] Undefined,
        //[Description("Intake")] Intake,
        //[Description("Exhaust")] Exhaust,
        [Description("Extract")] Extract,
        [Description("Supply")] Supply,
        //[Description("Return")] Return,
    }
}