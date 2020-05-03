using DynamicData;
using DynamicData.Binding;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public EHSIModel EHSI => new EHSIModel();

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
