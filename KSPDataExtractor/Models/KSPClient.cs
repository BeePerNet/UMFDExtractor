using KRPC.Client;
using KRPC.Client.Services.KRPC;
using KRPC.Client.Services.SpaceCenter;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace KSPDataExtractor.Models
{
    public class KSPClient : ReactiveObject, IDisposable
    {
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



        public ReactiveCommand<Unit, Unit> StartCommand { get; }
        public ReactiveCommand<Unit, Unit> StopCommand { get; }


        string status;
        public string Status
        {
            get => status;
            set => this.RaiseAndSetIfChanged(ref status, value);
        }

        bool running = false;
        public bool Running
        {
            get => running;
            private set => this.RaiseAndSetIfChanged(ref running, value);
        }

        GameScene? gameScene = null;
        public GameScene? GameScene
        {
            get => gameScene;
            private set => this.RaiseAndSetIfChanged(ref gameScene, value);
        }

        Waypoint[] waypoints = null;
        public Waypoint[] Waypoints
        {
            get => waypoints;
            private set => this.RaiseAndSetIfChanged(ref waypoints, value);
        }

        Waypoint waypoint;
        public Waypoint Waypoint
        {
            get => waypoint;
            private set => this.RaiseAndSetIfChanged(ref waypoint, value);
        }

        CelestialBody celestialBody;
        public CelestialBody CelestialBody
        {
            get => celestialBody;
            private set => this.RaiseAndSetIfChanged(ref celestialBody, value);
        }

        Vessel vessel;
        public Vessel Vessel
        {
            get => vessel;
            private set => this.RaiseAndSetIfChanged(ref vessel, value);
        }


        public KSPClient()
        {
            StartCommand = ReactiveCommand.Create(Start);
            StopCommand = ReactiveCommand.Create(Stop);
            Status = "Ready";
        }


        Stream<CelestialBody> bodystream;
        Stream<Vessel> vesselstream;
        Stream<GameScene> gamesceenstream;

        void Start()
        {
            Observable.Start(() =>
            {
                try
                {
                    Running = true;
                    Status = "Starting";

                    Connection = new Connection(nameof(KSPDataExtractor));
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
                                            bodystream.AddCallback((CelestialBody v) =>
                                            {
                                                CelestialBody = v;
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

                    Status = "Running";
                }
                catch (Exception ex)
                {
                    Stop();
                    Status = ex.Message;
                }
            });

        }

        void ReloadWaypoints()
        {
            if (Vessel != null && CelestialBody != null)
            {
                string vesselName = this.Vessel.Name;
                string bodyName = this.CelestialBody.Name;
                Waypoints = spaceCenter.WaypointManager.Waypoints.Where(T => T.Body.Name == bodyName && T.Name != vesselName).ToArray();
            }
            else
            {
                Waypoint = null;
                Waypoints = null;
            }
        }

        void Stop()
        {
            Status = "Stopping";
            Running = false;

            bodystream?.Remove();
            bodystream = null;
            vesselstream?.Remove();
            vesselstream = null;
            gamesceenstream?.Remove();
            gamesceenstream = null;

            GameScene = null;
            Vessel = null;
            CelestialBody = null;

            connection?.Dispose();
            connection = null;
            Status = "Stopped";
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
                Running = false;
                connection?.Dispose();

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

    }
}
