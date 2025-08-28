using Newtonsoft.Json.Linq;

namespace SAM 
 // namespace  SAM.Geometry.Spatial
{
    public static partial class Create
    {
        public static ICurve3D ICurve3D(this JObject jObject)
        {
            return Geometry.Create.ISAMGeometry(jObject) as ICurve3D;
        }
    }
}