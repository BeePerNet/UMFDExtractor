using System;
using System.Collections.Generic;
using System.Text;

namespace KSPDataExtractor.Models
{
    public class GeoTools
    {
        const double RAD_PER_DEG = (Math.PI / 180.0);

        const double DEG_PER_RAD = (180.0 / Math.PI);

        public static (double, double) CalculateDistance(double lat1, double long1, double meanAltitude1, double lat2, double long2, double meanAltitude2, double radius)
        {
            //Waypoint wpd = Waypoint;
            /*Vessel v = FlightGlobals.ActiveVessel;
            CelestialBody celestialBody = v.mainBody;

            // Simple distance
            if (Config.distanceCalcMethod == Config.DistanceCalcMethod.STRAIGHT_LINE || celestialBody != wpd.celestialBody)
            {
                return GetStraightDistance(wpd);
            }*/

            // Use the haversine formula to calculate great circle distance.
            double sin1 = Math.Sin(RAD_PER_DEG * (lat1 - lat2) / 2);
            double sin2 = Math.Sin(RAD_PER_DEG * (long1 - long2) / 2);
            double cos1 = Math.Cos(RAD_PER_DEG * lat2);
            double cos2 = Math.Cos(RAD_PER_DEG * lat1);

            double lateralDistance = 2 * (radius + meanAltitude2) *
                Math.Asin(Math.Sqrt(sin1 * sin1 + cos1 * cos2 * sin2 * sin2));

            double heightDist = Math.Abs(meanAltitude2 - meanAltitude1);

            if (/*Config.distanceCalcMethod == Config.DistanceCalcMethod.LATERAL || */heightDist <= lateralDistance / 2.0)
            {
                return (lateralDistance, lateralDistance);
            }
            else
            {
                // Get the ratio to use in our formula
                double z = (heightDist - lateralDistance / 2.0) / lateralDistance;

                // x / (x + 1) starts at 0 when x = 0, and increases to 1

                return ((z / (z + 1)) * heightDist + lateralDistance, lateralDistance);
            }
        }

        public static double CalculateBearing(double lat1, double lon1, double lat2, double lon2)
        {
            /*double dLon = long2 - long1;
            double y = Math.Sin(dLon) * Math.Cos(lat2);
            double x = Math.Cos(lat1) * Math.Sin(lat2) -
                       Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
            double bearing = Math.Atan2(y, x) * DEG_PER_RAD;*/
            /*if (bearing < 0)
            {
                bearing = Math.Abs(bearing) - 90;
                if (bearing < 0)
                    bearing += 360;
            }
            else
                bearing = 180 - bearing + 90;*/

            var dLon = (lon2 - lon1) * RAD_PER_DEG;
            var dPhi = Math.Log(
                Math.Tan((lat2 * RAD_PER_DEG) / 2 + Math.PI / 4) / Math.Tan((lat1 * RAD_PER_DEG) / 2 + Math.PI / 4));
            if (Math.Abs(dLon) > Math.PI)
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            return (Math.Atan2(dLon, dPhi) * DEG_PER_RAD + 360) % 360;
        }

        public static (double, double) CalculateDestination(double latitude, double longitude, double bearing, double distance, double radius)
        {
            double rad = bearing * RAD_PER_DEG;

            double angDist = distance / radius;


            latitude *= Math.PI / 180;
            longitude *= Math.PI / 180;

            double lat2 = Math.Asin(Math.Sin(latitude) * Math.Cos(angDist) + Math.Cos(latitude) * Math.Sin(angDist) * Math.Cos(rad));

            double forAtana = Math.Sin(rad) * Math.Sin(angDist) * Math.Cos(latitude);
            double forAtanb = Math.Cos(angDist) - Math.Sin(latitude) * Math.Sin(lat2);

            double lon2 = longitude + Math.Atan2(forAtana, forAtanb);

            lat2 *= DEG_PER_RAD;
            lon2 *= DEG_PER_RAD;

            return (lat2, lon2);
        }

        /// <summary>
        /// Gets the lateral distance in meters from the active vessel to the given waypoint.
        /// </summary>
        /// <param name="wpd">Activated waypoint</param>
        /// <returns>Distance in meters</returns>
        public static double CalculateLateralDistance(double lat1, double long1, double lat2, double long2, double radius)
        {
            // Use the haversine formula to calculate great circle distance.
            double sin1 = Math.Sin(RAD_PER_DEG * (lat1 - lat2) / 2);
            double sin2 = Math.Sin(RAD_PER_DEG * (long1 - long2) / 2);
            double cos1 = Math.Cos(RAD_PER_DEG * lat2);
            double cos2 = Math.Cos(RAD_PER_DEG * lat1);

            return 2 * (radius) *
                Math.Asin(Math.Sqrt(sin1 * sin1 + cos1 * cos2 * sin2 * sin2));
        }


        // returns 1 if otherAngle is to the right of sourceAngle,
        //         0 if the angles are identical
        //         -1 if otherAngle is to the left of sourceAngle
        public static bool CompareAngles(double sourceAngle, double otherAngle)
        {
            // sourceAngle and otherAngle should be in the range -180 to 180
            double difference = otherAngle - sourceAngle;

            if (difference < -180.0d)
                difference += 360.0d;
            if (difference > 180.0d)
                difference -= 360.0d;

            if (difference > 0.0d)
                return true;
            if (difference < 0.0d)
                return false;

            return true;
        }

    }
}
