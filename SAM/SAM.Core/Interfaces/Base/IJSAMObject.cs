using Newtonsoft.Json.Linq;

namespace SAM 
 // namespace  SAM.Core
{
    public interface IJSAMObject
    {
        bool FromJObject(JObject jObject);

        JObject ToJObject();
    }
}