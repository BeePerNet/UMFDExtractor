using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMFDExtractor.Models
{
    public class Waypoint : ReactiveObject
    {
        public Waypoint() { }
        public Waypoint(WaypointType type, string name, double latitude, double longitude, double altitude, double? bearing = null, string region = null, string airport = null)
        {
            Type = type;
            Name = name;
            Region = region;
            Airport = airport;
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
            Bearing = bearing;
        }

        WaypointType type;
        [Required]
        public WaypointType Type
        {
            get => type;
            set => this.RaiseAndSetIfChanged(ref type, value);
        }

        string name;
        [Required]
        public string Name
        {
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value);
        }

        string region;
        public string Region
        {
            get => region;
            set => this.RaiseAndSetIfChanged(ref region, value);
        }

        string airport;
        public string Airport
        {
            get => airport;
            set => this.RaiseAndSetIfChanged(ref airport, value);
        }


        public double latitude;

        [Required]
        public double Latitude
        {
            get => latitude;
            set
            {
                this.RaiseAndSetIfChanged(ref latitude, value);
                this.RaisePropertyChanged(nameof(MapUrl));
            }
        }
        public double longitude;
        [Required]
        public double Longitude
        {
            get => longitude;
            set
            {
                this.RaiseAndSetIfChanged(ref longitude, value);
                this.RaisePropertyChanged(nameof(MapUrl));
            }
        }
        public double altitude;
        [Required]
        public double Altitude
        {
            get => altitude;
            set => this.RaiseAndSetIfChanged(ref altitude, value);
        }


        public double? bearing;
        public double? Bearing
        {
            get => bearing;
            set => this.RaiseAndSetIfChanged(ref bearing, value);
        }

        public Uri MapUrl
        {
            get => new Uri(string.Format(CultureInfo.InvariantCulture, "http://maps.google.com/maps?q={0},{1}", latitude, longitude));
        }


    }
}
