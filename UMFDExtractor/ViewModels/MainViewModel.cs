using UMFDExtractor.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using UMFDExtractor.Windows;

namespace UMFDExtractor.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ReadOnlyDictionary<string, Type> Clients { get; }


        KSPValuesWindow kspValuesWindow;
        EHSIWindow ehsiWindow;
        WaypointsWindow waypointsWindow;


        public ClientBase client;
        public ClientBase Client
        {
            get => client;
            private set => this.RaiseAndSetIfChanged(ref client, value);
        }

        public Type selectedClient;

        public Type SelectedClient
        {
            get => selectedClient;
            set => this.RaiseAndSetIfChanged(ref selectedClient, value);
        }

        public MainViewModel(bool designtime) : base()
        {
            Dictionary<string, Type> clients = new Dictionary<string, Type>();
            clients.Add("Dummy", typeof(Models.DummyClient));
            clients.Add("X-Plane", typeof(Models.XPlane.XPlaneClient));
            clients.Add("Kerbal Space Program", typeof(Models.KSP.KSPClient));
            clients.Add("Falcon BMS", typeof(Models.BMS.BMSClient));
            Clients = new ReadOnlyDictionary<string, Type>(clients);

            SelectedClient = clients.First().Value;

            OpenKSPValuesWindowCommand = ReactiveCommand.Create(OpenKSPValuesWindow, this.WhenAnyValue(x => x.Client).Select(x => x is Models.KSP.KSPClient).ObserveOnDispatcher());
            OpenEHSIWindowCommand = ReactiveCommand.Create(OpenEHSIWindow, this.WhenAnyValue(x => x.Client).Select(x => x is IEHSIProvider).ObserveOnDispatcher());
            OpenWaypointsWindowCommand = ReactiveCommand.Create(OpenWaypointsWindow, this.WhenAnyValue(x => x.Client).Select(x => x != null).ObserveOnDispatcher());
            OpenPropertiesWindowCommand = ReactiveCommand.Create(OpenPropertiesWindow, this.WhenAnyValue(x => x.Client).Select(x => x != null).ObserveOnDispatcher());


            if (designtime)
            {
                Client = new DummyClient();
            }
            else
                this.WhenAnyValue(x => x.SelectedClient).Subscribe(SelectedClientChanged);
        }

        public MainViewModel() : this(true)
        {
        }

        void SelectedClientChanged(Type c)
        {
            Client?.Dispose();

            if (c != null && (Client == null || !Client.Running))
            {
                Client = (ClientBase)Activator.CreateInstance(c);

                (Client as ILoadable)?.Load();
            }
        }

        public ReactiveCommand<Unit, Unit> OpenKSPValuesWindowCommand { get; }

        void OpenKSPValuesWindow()
        {
            if (kspValuesWindow == null || !kspValuesWindow.IsActive)
            {
                kspValuesWindow = new KSPValuesWindow();
                kspValuesWindow.DataContext = new KSPValuesWindowViewModel(Client as Models.KSP.KSPClient);
                kspValuesWindow.Show();
            }
        }

        public ReactiveCommand<Unit, Unit> OpenEHSIWindowCommand { get; }
        void OpenEHSIWindow()
        {
            if (ehsiWindow == null || !ehsiWindow.IsActive)
            {
                ehsiWindow = new EHSIWindow(this);
                ehsiWindow.Show();
            }
        }

        public ReactiveCommand<Unit, Unit> OpenWaypointsWindowCommand { get; }
        void OpenWaypointsWindow()
        {
            if (waypointsWindow == null || !waypointsWindow.IsActive)
            {
                waypointsWindow = new WaypointsWindow(this);
                waypointsWindow.Show();
            }
        }

        public ReactiveCommand<Unit, Unit> OpenPropertiesWindowCommand { get; }

        PropertiesWindow propertiesWindow;
        void OpenPropertiesWindow()
        {
            if (propertiesWindow == null || !propertiesWindow.IsActive)
            {
                propertiesWindow = new PropertiesWindow();
                propertiesWindow.SetDisposable(this.WhenAnyValue(x => x.Client).Subscribe(x => propertiesWindow.DataContext = x));
                propertiesWindow.Show();
            }
        }



    }
}
