using UMFDExtractor.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;

namespace UMFDExtractor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
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

        public MainWindowViewModel(bool designtime) : base()
        {
            Dictionary<string, Type> clients = new Dictionary<string, Type>();
            clients.Add("X-Plane", typeof(Models.XPlane.XPlaneClient));
            clients.Add("Kerbal Space Program", typeof(Models.KSP.KSPClient));
            clients.Add("Falcon BMS", typeof(Models.BMS.BMSClient));
            Clients = new ReadOnlyDictionary<string, Type>(clients);

            OpenKSPValuesWindowCommand = ReactiveCommand.Create(OpenKSPValuesWindow);
            OpenEHSIWindowCommand = ReactiveCommand.Create(OpenEHSIWindow);
            OpenWaypointsWindowCommand = ReactiveCommand.Create(OpenWaypointsWindow);


            if (designtime)
                Client = new Models.XPlane.XPlaneClient();
            else
                this.WhenAnyValue(x => x.SelectedClient).Subscribe(SelectedClientChanged);
        }

        public MainWindowViewModel() : this(true)
        {
        }

        void SelectedClientChanged(Type c)
        {
            if (c != null && (Client == null || !Client.Running))
            {
                Client = (ClientBase)Activator.CreateInstance(c);

                (Client as ILoadable)?.Load();

                if (ehsiWindow != null)
                    (ehsiWindow.DataContext as EHSIWindowViewModel).SetClient(Client as IEHSIProvider);
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
                ehsiWindow = new EHSIWindow();
                ehsiWindow.DataContext = new EHSIWindowViewModel(Client as IEHSIProvider);
                ehsiWindow.Show();
            }
        }

        public ReactiveCommand<Unit, Unit> OpenWaypointsWindowCommand { get; }
        void OpenWaypointsWindow()
        {
            if (waypointsWindow == null || !waypointsWindow.IsActive)
            {
                waypointsWindow = new WaypointsWindow();
                waypointsWindow.DataContext = new WaypointsWindowViewModel(Client);
                waypointsWindow.Show();
            }
        }

        public void Close()
        {
            kspValuesWindow?.Close();
            ehsiWindow?.Close();
            waypointsWindow?.Close();

            Client?.Dispose();
        }
    }
}
