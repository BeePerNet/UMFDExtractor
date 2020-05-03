using KRPC.Client;
using KRPC.Client.Services.SpaceCenter;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace UMFDExtractor.Models.KSP
{
    public partial class KSPClient : IEHSIProvider
    {
        [Category("Values")]
        [ExpandableObject]
        public EHSIModel EHSI { get; } = new EHSIModel();


        IDisposable runningSubscription;
        IDisposable gameSceneSubscription;
        IDisposable vesselSubscription;
        IDisposable bodySubscription;
        IDisposable waypointSubscription;

        Stream<float> flightstream;
        Stream<double> longitudestream;
        Stream<double> altitudestream;
        Stream<double> latitudestream;

        public void StartEHSI()
        {
            Observable.Start(() =>
            {
                try
                {
                    if (runningSubscription == null)
                    {
                        runningSubscription = this.WhenAnyValue(x => x.Running).Subscribe(x =>
                        {
                            if (x)
                                this.InternalStartEHSI();
                            else
                                this.InternalStopEHSI();
                        });

                        gameSceneSubscription = this.ObservableForProperty(x => x.GameScene, false, true).Subscribe(x =>
                        {
                            if (x.Value.HasValue && x.Value.Value == KRPC.Client.Services.KRPC.GameScene.Flight)
                                this.InternalStartEHSI();
                            else
                                this.InternalStopEHSI();
                        });

                        vesselSubscription = this.ObservableForProperty(x => x.Vessel, false, true).Subscribe(x =>
                        {
                            this.InternalStopEHSI();
                            if (x != null)
                                this.InternalStartEHSI();
                        });

                        bodySubscription = this.WhenAnyValue(x => x.CelestialBody).Subscribe(x => EHSI.BodyRadius = x?.EquatorialRadius ?? 0);

                        waypointSubscription = this.WhenAnyValue(x => x.Waypoint).Subscribe(x =>
                        {
                            EHSI.WorkingWaypoint = Waypoint != null && Vessel != null;
                            if (EHSI.WorkingWaypoint)
                            {
                                EHSI.WaypointLatitude = x.Latitude;
                                EHSI.WaypointLongitude = x.Longitude;
                                EHSI.WaypointMeanAltitude = x.Altitude;

                                if (x.Bearing.HasValue)
                                    EHSI.Bearing = x.Bearing.Value;
                            }
                            else
                                EHSI.Distance = 0;
                        });
                    }
                }
                catch (Exception ex)
                {
                    Status = ex.Message;
                }
            });
        }

        bool startingEhsi = false;

        void InternalStartEHSI()
        {
            if (!startingEhsi && !EHSI.Running && Running && GameScene == KRPC.Client.Services.KRPC.GameScene.Flight && Vessel != null)
            {
                startingEhsi = true;
                Task task = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var flight = Vessel.Flight(Vessel.SurfaceReferenceFrame);

                        flightstream = Connection.AddStream(() => flight.Heading);
                        flightstream.AddCallback((float h) => EHSI.FlightHeading = h);
                        flightstream.Start(false);

                        longitudestream = Connection.AddStream(() => flight.Longitude);
                        longitudestream.AddCallback((double l) => EHSI.Longitude = l);
                        longitudestream.Start(false);

                        altitudestream = Connection.AddStream(() => flight.MeanAltitude);
                        altitudestream.AddCallback((double a) => EHSI.MeanAltitude = a);
                        altitudestream.Start(false);

                        latitudestream = Connection.AddStream(() => flight.Latitude);
                        latitudestream.AddCallback((double l) =>
                        {
                            EHSI.Latitude = l;
                            try
                            {
                                EHSI.CalculateWaypoint();
                            }
                            catch (Exception ex)
                            {
                                Status = ex.Message;
                            }
                        });
                        latitudestream.Start();
                        EHSI.Running = true;
                    }
                    catch (Exception ex)
                    {
                        Status = ex.Message;
                    }
                    startingEhsi = false;
                });
                if (!task.Wait(10000))
                    task.GetAwaiter();
            }
        }

        void InternalStopEHSI()
        {
            bodystream?.Remove();
            bodystream = null;
            vesselstream?.Remove();
            vesselstream = null;
            gamesceenstream?.Remove();
            gamesceenstream = null;

            EHSI.Running = false;
        }

        public void StopEHSI()
        {
            try
            {
                InternalStopEHSI();

                runningSubscription?.Dispose();
                gameSceneSubscription?.Dispose();
                vesselSubscription?.Dispose();
                bodySubscription?.Dispose();
                waypointSubscription?.Dispose();
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
        }


    }
}
