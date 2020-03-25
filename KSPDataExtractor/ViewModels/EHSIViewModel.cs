using KRPC.Client;
using KRPC.Client.Services.KRPC;
using KSPDataExtractor.Models;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace KSPDataExtractor.ViewModels
{
    public class EHSIViewModel : ViewModelBase, IDisposable
    {
        KSPClient Client { get; }


        double bodyRadius;
        public double BodyRadius
        {
            get => bodyRadius;
            private set => this.RaiseAndSetIfChanged(ref bodyRadius, value);
        }


        double distance;
        public double Distance
        {
            get => distance;
            private set => this.RaiseAndSetIfChanged(ref distance, value);
        }

        double heading;
        public double Heading
        {
            get => heading;
            private set => this.RaiseAndSetIfChanged(ref heading, value);
        }

        float flightHeading;
        public float FlightHeading
        {
            get => flightHeading;
            private set => this.RaiseAndSetIfChanged(ref flightHeading, value);
        }

        double course;
        public double Course
        {
            get => course;
            private set => this.RaiseAndSetIfChanged(ref course, value);
        }

        double bearing;
        public double Bearing
        {
            get => bearing;
            private set => this.RaiseAndSetIfChanged(ref bearing, value);
        }


        bool workingWaypoint = false;
        public bool WorkingWaypoint
        {
            get => workingWaypoint;
            private set => this.RaiseAndSetIfChanged(ref workingWaypoint, value);
        }

        double courseDeviation;
        public double CourseDeviation
        {
            get => courseDeviation;
            private set => this.RaiseAndSetIfChanged(ref courseDeviation, value);
        }

        double courseDeviationLog;
        public double CourseDeviationLog
        {
            get => courseDeviationLog;
            private set => this.RaiseAndSetIfChanged(ref courseDeviationLog, value);
        }

        double waypointLatitude;
        public double WaypointLatitude
        {
            get => waypointLatitude;
            private set => this.RaiseAndSetIfChanged(ref waypointLatitude, value);
        }

        double waypointLongitude;
        public double WaypointLongitude
        {
            get => waypointLongitude;
            private set => this.RaiseAndSetIfChanged(ref waypointLongitude, value);
        }

        double waypointMeanAltitude;
        public double WaypointMeanAltitude
        {
            get => waypointMeanAltitude;
            private set => this.RaiseAndSetIfChanged(ref waypointMeanAltitude, value);
        }

        double latitude;
        public double Latitude
        {
            get => latitude;
            private set => this.RaiseAndSetIfChanged(ref latitude, value);
        }

        double longitude;
        public double Longitude
        {
            get => longitude;
            private set => this.RaiseAndSetIfChanged(ref longitude, value);
        }

        double meanAltitude;
        public double MeanAltitude
        {
            get => meanAltitude;
            private set => this.RaiseAndSetIfChanged(ref meanAltitude, value);
        }



        public EHSIViewModel()
        {
            this.Client = new KSPClient();
        }

        public EHSIViewModel(KSPClient kspclient)
        {
            Client = kspclient;

            kspclient.WhenAnyValue(x => x.Running).Subscribe(x =>
            {
                if (x)
                    this.Start();
                else
                    this.Stop();
            });

            kspclient.WhenAnyValue(x => x.GameScene).Subscribe(x =>
            {
                if (x == GameScene.Flight)
                    this.Start();
                else
                    this.Stop();
            });

            kspclient.WhenAnyValue(x => x.Vessel).Subscribe(x =>
            {
                this.Stop();
                if (x != null)
                    this.Start();
            });

            kspclient.WhenAnyValue(x => x.CelestialBody).Subscribe(x => BodyRadius = x?.EquatorialRadius ?? 0);

            kspclient.WhenAnyValue(x => x.Waypoint).Subscribe(x =>
            {
                WorkingWaypoint = Client.Waypoint != null && Client.Vessel != null;
                if (WorkingWaypoint)
                {
                    WaypointLatitude = x.Latitude;
                    WaypointLongitude = x.Longitude;
                    WaypointMeanAltitude = x.MeanAltitude;
                }
                else
                    Distance = 0;
            });
        }

        Stream<float> flightstream;
        Stream<double> longitudestream;
        Stream<double> altitudestream;
        Stream<double> latitudestream;

        void Stop()
        {
            flightstream?.Remove();
            flightstream = null;
            longitudestream?.Remove();
            longitudestream = null;
            altitudestream?.Remove();
            altitudestream = null;
            latitudestream?.Remove();
            latitudestream = null;
        }

        void Start()
        {
            Observable.Start(() =>
            {
                if (Client.Running && Client.GameScene == GameScene.Flight && Client.Vessel != null)
                {
                    try
                    {
                        var flight = Client.Vessel.Flight(Client.Vessel.SurfaceReferenceFrame);

                        flightstream = Client.Connection.AddStream(() => flight.Heading);
                        flightstream.AddCallback((float h) => FlightHeading = h);
                        flightstream.Start();

                        longitudestream = Client.Connection.AddStream(() => flight.Longitude);
                        longitudestream.AddCallback((double l) => Longitude = l);
                        longitudestream.Start();

                        altitudestream = Client.Connection.AddStream(() => flight.MeanAltitude);
                        altitudestream.AddCallback((double a) => MeanAltitude = a);
                        altitudestream.Start();

                        latitudestream = Client.Connection.AddStream(() => flight.Latitude);
                        latitudestream.AddCallback((double l) =>
                        {
                            Latitude = l;
                            if (WorkingWaypoint)
                            {
                                try
                                {
                                    var result = GeoTools.CalculateDistance(Latitude, Longitude, MeanAltitude, WaypointLatitude, WaypointLongitude, WaypointMeanAltitude, BodyRadius);
                                    Distance = result.Item1;
                                    double waypointLateralDistance = result.Item2;

                                    Bearing = GeoTools.CalculateBearing(Latitude, Longitude, WaypointLatitude, WaypointLongitude);

                                    (double, double) destination = GeoTools.CalculateDestination(WaypointLatitude, WaypointLongitude, Course + 180, waypointLateralDistance, BodyRadius);

                                    CourseDeviation = GeoTools.CalculateLateralDistance(Latitude, Longitude, destination.Item1, destination.Item2, BodyRadius);

                                    CourseDeviationLog = Math.Min(Math.Log(CourseDeviation / 100 + 1, 1.02) / 5, 52) * (GeoTools.CompareAngles(Course, Bearing) ? 1 : -1);
                                }
                                catch (Exception ex)
                                {
                                    Client.Status = ex.Message;
                                }
                            }
                        });
                        latitudestream.Start();
                    }
                    catch (Exception ex)
                    {
                        Client.Status = ex.Message;
                    }
                }
            });
        }

        // some fields that require cleanup
        private bool disposed = false; // to detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {

                }
                Stop();

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }



    }
}
