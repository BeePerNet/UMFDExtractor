using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMFDExtractor.Models;

namespace UMFDExtractor.ViewModels
{
    public class WaypointsWindowViewModel : ViewModelBase
    {
        public ClientBase Client { get; }

        public WaypointsWindowViewModel()
        {
            Client = new Models.XPlane.XPlaneClient();
            Client.Waypoints.Add(new Waypoint(WaypointType.Waypoint, "ZeeRo", 0, 0, 0));
            Client.Waypoints.Add(new Waypoint(WaypointType.Waypoint, "Québec", 46.8073, -71.2072, 65, null, "CY", "CYQB"));
        }
        public WaypointsWindowViewModel(ClientBase client)
        {
            Client = client;
            if (client.Waypoints.Count == 0)
            {
                Client.Waypoints.Add(new Waypoint(WaypointType.Waypoint, "ZeeRo", 0, 0, 0));
                Client.Waypoints.Add(new Waypoint(WaypointType.Waypoint, "Québec", 46.8073, -71.2072, 65, null, "CY", "CYQB"));
            }
        }


    }
}
