using System.ComponentModel;

namespace SAM 
 // namespace  SAM.Core
{
    [Description("Page Orientation")]
    public enum PageOrientation
    {
        [Description("Undefined")] Undefined,
        [Description("Portrait")] Portrait,
        [Description("Landscape")] Landscape,
    }
}