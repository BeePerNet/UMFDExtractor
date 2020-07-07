using DynamicData.Binding;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace UMFDExtractor.Models
{
    public abstract class ClientBase: ReactiveObjectBase, IClient
    {
        [Browsable(false)]
        public ReactiveCommand<Unit, Unit> StartCommand { get; protected set; }

        [Browsable(false)]
        public ReactiveCommand<Unit, Unit> StopCommand { get; protected set; }

        bool running = false;
        [Category("Status")]
        public bool Running
        {
            get => running;
            protected set => this.RaiseAndSetIfChanged(ref running, value);
        }

        string status = "Stopped";
        [Category("Status")]
        public string Status
        {
            get => status;
            set => this.RaiseAndSetIfChanged(ref status, value);
        }

        bool canStart = true;
        [Category("Status")]
        public bool CanStart
        {
            get => canStart;
            set => this.RaiseAndSetIfChanged(ref canStart, value);
        }

        public ClientBase()
        {
            Waypoints = new ObservableCollectionExtended<Waypoint>();

            StartCommand = ReactiveCommand.Create(Start, this.WhenAnyValue(x => x.CanStart).ObserveOnDispatcher());
            StopCommand = ReactiveCommand.Create(Stop, this.WhenAnyValue(x => x.Running).ObserveOnDispatcher());
        }

        protected abstract void Start();

        protected abstract void Stop();

        [Category("Data")]
        [ExpandableObject]
        [Browsable(false)]
        public ObservableCollectionExtended<Waypoint> Waypoints { get; }

        protected ReadOnlyObservableCollection<Waypoint> filteredWaypoints;
        [Category("Data")]
        [ExpandableObject]
        [Browsable(false)]
        public ReadOnlyObservableCollection<Waypoint> FilteredWaypoints { get => filteredWaypoints; }

        Waypoint waypoint;
        [ExpandableObject]
        [Category("Data")]
        public Waypoint Waypoint
        {
            get => waypoint;
            set => this.RaiseAndSetIfChanged(ref waypoint, value);
        }


        protected abstract void InternalDispose();

        // some fields that require cleanup
        private bool disposed = false; // to detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                }
                InternalDispose();

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }


    }
}
