using DynamicData;
using DynamicData.Binding;
using KRPC.Client;
using KRPC.Client.Services.KRPC;
using KRPC.Client.Services.SpaceCenter;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace UMFDExtractor.Models.KSP
{
    public partial class KSPClient : ClientBase
    {

        [ExpandableObject]
        public KSPClient()
        {
            Waypoints.ToObservableChangeSet().Filter(T => T.Region == this.CelestialBody.Name).Bind(out filteredWaypoints).Subscribe();
        }


        Connection connection;
        public Connection Connection
        {
            get => connection;
            private set => this.RaiseAndSetIfChanged(ref connection, value);
        }
        KRPC.Client.Services.SpaceCenter.Service spaceCenter;
        public KRPC.Client.Services.SpaceCenter.Service SpaceCenter
        {
            get => spaceCenter;
            private set => this.RaiseAndSetIfChanged(ref spaceCenter, value);
        }
        KRPC.Client.Services.KRPC.Service krpc;
        public KRPC.Client.Services.KRPC.Service Krpc
        {
            get => krpc;
            private set => this.RaiseAndSetIfChanged(ref krpc, value);
        }


        GameScene? gameScene = null;
        [Category("Values")]
        public GameScene? GameScene
        {
            get => gameScene;
            private set => this.RaiseAndSetIfChanged(ref gameScene, value);
        }


        CelestialBody celestialBody;
        [Category("Values")]
        public CelestialBody CelestialBody
        {
            get => celestialBody;
            private set => this.RaiseAndSetIfChanged(ref celestialBody, value);
        }

        Vessel vessel;
        [Category("Values")]
        public Vessel Vessel
        {
            get => vessel;
            private set => this.RaiseAndSetIfChanged(ref vessel, value);
        }


        Stream<CelestialBody> bodystream;
        Stream<Vessel> vesselstream;
        Stream<GameScene> gamesceenstream;

        bool starting = false;
        protected override void Start()
        {
            if (!starting)
            {
                starting = true;
                Observable.Start(() =>
                {
                    try
                    {
                        Status = "Starting";

                        Connection = new Connection(nameof(UMFDExtractor));
                        Krpc = connection.KRPC();
                        SpaceCenter = connection.SpaceCenter();

                        gamesceenstream = connection.AddStream(() => krpc.CurrentGameScene);
                        gamesceenstream.AddCallback((GameScene s) =>
                        {
                            this.GameScene = s;
                            Observable.Start(() =>
                            {
                                if (GameScene == KRPC.Client.Services.KRPC.GameScene.Flight)
                                {
                                    vesselstream = connection.AddStream(() => spaceCenter.ActiveVessel);
                                    vesselstream.AddCallback((Vessel v) =>
                                    {
                                        Vessel = v;

                                        Observable.Start(() =>
                                        {
                                            if (Vessel != null)
                                            {
                                                bodystream = connection.AddStream(() => spaceCenter.ActiveVessel.Orbit.Body);
                                                bodystream.AddCallback((CelestialBody c) =>
                                                {
                                                    CelestialBody = c;
                                                    this.ReloadWaypoints();
                                                });
                                                bodystream.Start();
                                            }
                                            else
                                            {
                                                bodystream?.Remove();
                                                bodystream = null;
                                            }
                                            this.ReloadWaypoints();
                                        });
                                    });
                                    vesselstream.Start();
                                }
                                else
                                {
                                    bodystream?.Remove();
                                    bodystream = null;
                                    vesselstream?.Remove();
                                    vesselstream = null;
                                }
                            });
                        });
                        gamesceenstream.Start();

                        Running = true;
                        Status = "Running";
                    }
                    catch (Exception ex)
                    {
                        Stop();
                        Status = ex.Message;
                    }
                    starting = false;
                });
            }

        }

        void ReloadWaypoints()
        {
            if (Vessel != null && CelestialBody != null)
            {
                string vesselName = this.Vessel.Name;
                string bodyName = this.CelestialBody.Name;

                Observable.Start(() =>
                {
                    Waypoints.Clear();
                    Waypoints.AddRange(spaceCenter.WaypointManager.Waypoints.Select(x => 
                        new Waypoint(WaypointType.Waypoint, x.Name, x.Latitude, x.Longitude, x.MeanAltitude, null, bodyName, x.Icon)));
                }, RxApp.MainThreadScheduler);
            }
            else
            {
                Waypoint = null;
                Waypoints.Clear();
            }
        }

        protected override void Stop()
        {
            try
            {
                starting = false;
                //Status = "Stopping";

                InternalStop();
                GameScene = null;
                Vessel = null;
                CelestialBody = null;

                Status = "Stopped";
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
        }

        void InternalStop()
        {
            Running = false;

            flightstream?.Remove();
            flightstream = null;
            longitudestream?.Remove();
            longitudestream = null;
            altitudestream?.Remove();
            altitudestream = null;
            latitudestream?.Remove();
            latitudestream = null;

            connection?.Dispose();
            connection = null;
        }


        protected override void InternalDispose()
        {
            InternalStop();
        }









    }
}
