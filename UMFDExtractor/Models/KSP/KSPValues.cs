using KRPC.Client.Services.KRPC;
using UMFDExtractor.Models.KSP;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using System.ComponentModel;

namespace UMFDExtractor.Models.KSP
{
    public class KSPValues : ReactiveObject, IDisposable
    {
        KSPClient Client { get; }

        [Category("Values")]
        [ExpandableObject]
        public KSPOrbit Orbit { get; } = new KSPOrbit();
        [Category("Values")]
        [ExpandableObject]
        public KSPFlight SurfaceFlight { get; } = new KSPFlight();



        public KSPValues()
        {
            this.Client = new KSPClient();
        }


        public KSPValues(KSPClient kspclient)
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
        }

        bool run;

        void Start()
        {
            Observable.Start(() =>
            {
                if (Client.Running && Client.GameScene == GameScene.Flight && Client.Vessel != null)
                {
                    run = true;
                    try
                    {
                        var flight = Client.Vessel.Flight(Client.Vessel.SurfaceReferenceFrame);

                        while(run)
                        {
                            try
                            {
                                Orbit.Update(Client.Vessel.Orbit);
                                SurfaceFlight.Update(flight);
                            }
                            catch (Exception ex)
                            {
                                Client.Status = ex.Message;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Client.Status = ex.Message;
                    }
                }
            });
        }

        void Stop()
        {
            run = false;
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
