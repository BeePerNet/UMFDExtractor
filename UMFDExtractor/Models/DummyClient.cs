using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace UMFDExtractor.Models
{
    public class DummyClient : ClientBase, IEHSIProvider
    {
        IDisposable waypointSubscription;

        public DummyClient()
        {
            CanStart = false;

            Waypoints.Add(new Waypoint(WaypointType.Waypoint, "ZeeRo", 0, 0, 0));
            Waypoints.Add(new Waypoint(WaypointType.Waypoint, "Québec", 46.8073, -71.2072, 65, null, "CY", "CYQB"));

            filteredWaypoints = new ReadOnlyObservableCollection<Waypoint>(Waypoints);

            waypointSubscription = this.WhenAnyValue(x => x.Waypoint).Subscribe(x =>
            {
                EHSI.WorkingWaypoint = Waypoint != null;
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

            Waypoint = Waypoints.First();

            EHSI.PropertyChanged += EHSI_PropertyChanged;
        }



        private void EHSI_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            EHSI.CalculateWaypoint();
        }

        [Category("Values")]
        [ExpandableObject]
        public EHSIModel EHSI { get; } = new EHSIModel();

        public void StartEHSI()
        {
        }

        public void StopEHSI()
        {
        }

        protected override void InternalDispose()
        {
        }

        protected override void Start()
        {
        }

        protected override void Stop()
        {
        }
    }
}
