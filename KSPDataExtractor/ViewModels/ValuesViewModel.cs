using KRPC.Client.Services.KRPC;
using KSPDataExtractor.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace KSPDataExtractor.ViewModels
{
    public class ValuesViewModel : ViewModelBase, IDisposable
    {
        KSPClient Client { get; }
        public KSPOrbit Orbit { get; } = new KSPOrbit();
        public KSPFlight SurfaceFlight { get; } = new KSPFlight();



        public ValuesViewModel()
        {
            this.Client = new KSPClient();
        }


        public ValuesViewModel(KSPClient kspclient)
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
