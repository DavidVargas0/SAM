using System.ComponentModel;

namespace SAM 
 // namespace  SAM.Core
{
    [Description("Log Record Type")]
    public enum LogRecordType
    {
        [Description("Undefined")] Undefined,
        [Description("Message")] Message,
        [Description("Warning")] Warning,
        [Description("Error")] Error
    }
}