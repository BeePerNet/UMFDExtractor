using ReactiveUI;
using System;
using System.ComponentModel;

namespace UMFDExtractor.Models
{
    public class EHSIModel : ReactiveObjectBase
    {
        public EHSIModel() : base() { }

        bool running = false;
        [Category("Status")]
        public bool Running
        {
            get => running;
            set => this.RaiseAndSetIfChanged(ref running, value);
        }

        double bodyRadius = 6378100;
        [Category("Data")]
        public double BodyRadius
        {
            get => bodyRadius;
            set => this.RaiseAndSetIfChanged(ref bodyRadius, value);
        }


        double distance;
        [Category("Data")]
        public double Distance
        {
            get => distance;
            set => this.RaiseAndSetIfChanged(ref distance, value);
        }

        double heading;
        [Category("Data")]
        public double Heading
        {
            get => heading;
            set => this.RaiseAndSetIfChanged(ref heading, value);
        }

        float flightHeading = 0;
        [Category("Data")]
        public float FlightHeading
        {
            get => flightHeading;
            set => this.RaiseAndSetIfChanged(ref flightHeading, value);
        }

        double course;
        [Category("Data")]
        public double Course
        {
            get => course;
            set => this.RaiseAndSetIfChanged(ref course, value);
        }

        double bearing;
        [Category("Data")]
        public double Bearing
        {
            get => bearing;
            set => this.RaiseAndSetIfChanged(ref bearing, value);
        }


        bool isEnabled = true;
        [Category("Data")]
        public bool IsEnabled
        {
            get => isEnabled;
            set => this.RaiseAndSetIfChanged(ref isEnabled, value);
        }


        bool workingWaypoint;
        [Category("Data")]
        public bool WorkingWaypoint
        {
            get => workingWaypoint;
            set => this.RaiseAndSetIfChanged(ref workingWaypoint, value);
        }

        double courseDeviation;
        [Category("Data")]
        public double CourseDeviation
        {
            get => courseDeviation;
            set => this.RaiseAndSetIfChanged(ref courseDeviation, value);
        }

        double courseDeviationLog;
        [Category("Data")]
        public double CourseDeviationLog
        {
            get => courseDeviationLog;
            set => this.RaiseAndSetIfChanged(ref courseDeviationLog, value);
        }

        double waypointLatitude;
        [Category("Data")]
        public double WaypointLatitude
        {
            get => waypointLatitude;
            set => this.RaiseAndSetIfChanged(ref waypointLatitude, value);
        }

        double waypointLongitude;
        [Category("Data")]
        public double WaypointLongitude
        {
            get => waypointLongitude;
            set => this.RaiseAndSetIfChanged(ref waypointLongitude, value);
        }

        double waypointMeanAltitude;
        [Category("Data")]
        public double WaypointMeanAltitude
        {
            get => waypointMeanAltitude;
            set => this.RaiseAndSetIfChanged(ref waypointMeanAltitude, value);
        }

        double latitude;
        [Category("Data")]
        public double Latitude
        {
            get => latitude;
            set => this.RaiseAndSetIfChanged(ref latitude, value);
        }

        double longitude;
        [Category("Data")]
        public double Longitude
        {
            get => longitude;
            set => this.RaiseAndSetIfChanged(ref longitude, value);
        }

        double meanAltitude;
        [Category("Data")]
        public double MeanAltitude
        {
            get => meanAltitude;
            set => this.RaiseAndSetIfChanged(ref meanAltitude, value);
        }

        public void CalculateWaypoint()
        {
            if (WorkingWaypoint)
            {
                var result = GeoTools.CalculateDistance(Latitude, Longitude, MeanAltitude, WaypointLatitude, WaypointLongitude, WaypointMeanAltitude, BodyRadius);
                Distance = result.Item1;

                Bearing = GeoTools.CalculateBearing(Latitude, Longitude, WaypointLatitude, WaypointLongitude);

                CalculateCourseDeviation(result.Item2);
            }
        }

        public void CalculateCourseDeviation(double? lateralDistance = null)
        {
            if (!lateralDistance.HasValue)
                lateralDistance = GeoTools.CalculateLateralDistance(Latitude, Longitude, WaypointLatitude, WaypointLongitude, BodyRadius);

            (double, double) destination = GeoTools.CalculateDestination(WaypointLatitude, WaypointLongitude, Course + 180, lateralDistance.Value, BodyRadius);

            CourseDeviation = GeoTools.CalculateLateralDistance(Latitude, Longitude, destination.Item1, destination.Item2, BodyRadius);

            CalculateCourseDeviationLog();
        }

        public void CalculateCourseDeviationLog()
        {
            CourseDeviationLog = Math.Min(Math.Log(Math.Abs(CourseDeviation) / 100 + 1, 1.02) / 3, 80) * (GeoTools.CompareAngles(Course, Bearing) ? 1 : -1);
        }

    }
}
