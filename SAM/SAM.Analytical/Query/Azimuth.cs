﻿namespace SAM 
 // namespace  SAM.Analytical
{
    public static partial class Query
    {
        /// <summary>
        /// Azimuth of the panel expressed in degrees. Reference direction for calculated Azimuth and WorldY
        /// </summary>
        /// <param name="panel">Panel</param>
        /// <returns>Azimuth expressed in degrees</returns>
        public static double Azimuth(this Panel panel)
        {
            if (panel == null)
                return double.NaN;

            return Geometry.Spatial.Query.Azimuth(panel.GetFace3D(), Geometry.Spatial.Vector3D.WorldY);
        }

        /// <summary>
        /// Azimuth of the aperture expressed in degrees. Reference direction for calculated Azimuth and WorldY
        /// </summary>
        /// <param name="aperture">Panel</param>
        /// <returns>Azimuth expressed in degrees</returns>
        public static double Azimuth(this Aperture aperture)
        {
            if (aperture == null)
                return double.NaN;

            return Geometry.Spatial.Query.Azimuth(aperture.GetFace3D(), Geometry.Spatial.Vector3D.WorldY);
        }
    }
}