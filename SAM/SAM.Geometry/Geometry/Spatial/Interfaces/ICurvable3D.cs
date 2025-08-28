using System.Collections.Generic;

namespace SAM 
 // namespace  SAM.Geometry.Spatial
{
    public interface ICurvable3D : IBoundable3D
    {
        List<ICurve3D> GetCurves();
    }
}