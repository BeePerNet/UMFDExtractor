using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace UMFDExtractor.Models
{
    public class DummyClient : ClientBase, IEHSIProvider
    {
        public DummyClient()
        {
            CanStart = false;

            Waypoints.Add(new Waypoint(WaypointType.Waypoint, "ZeeRo", 0, 0, 0));
            Waypoints.Add(new Waypoint(WaypointType.Waypoint, "Québec", 46.8073, -71.2072, 65, null, "CY", "CYQB"));

            filteredWaypoints = new ReadOnlyObservableCollection<Waypoint>(Waypoints);

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
