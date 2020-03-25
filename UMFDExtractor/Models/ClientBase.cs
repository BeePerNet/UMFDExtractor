using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace UMFDExtractor.Models
{
    public abstract class ClientBase: ReactiveObject, IClient
    {
        [Browsable(false)]
        public ReactiveCommand<Unit, Unit> StartCommand { get; protected set; }

        [Browsable(false)]
        public ReactiveCommand<Unit, Unit> StopCommand { get; protected set; }

        bool running = false;
        [Category("States")]
        public bool Running
        {
            get => running;
            protected set => this.RaiseAndSetIfChanged(ref running, value);
        }

        string status = "Stopped";
        [Category("States")]
        public string Status
        {
            get => status;
            set => this.RaiseAndSetIfChanged(ref status, value);
        }

        public ClientBase()
        {
            StartCommand = ReactiveCommand.Create(Start);
            StopCommand = ReactiveCommand.Create(Stop);
        }

        protected abstract void Start();

        protected abstract void Stop();

        [Category("Values")]
        [ExpandableObject]
        [Browsable(false)]
        public ObservableCollectionExtended<Waypoint> Waypoints { get; } = new ObservableCollectionExtended<Waypoint>();

        [Category("Values")]
        [ExpandableObject]
        [Browsable(false)]
        public ObservableCollectionExtended<Waypoint> FilteredWaypoints { get; } = new ObservableCollectionExtended<Waypoint>();

        Waypoint waypoint;
        [ExpandableObject]
        [Category("Values")]
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
