using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace UMFDExtractor.Models.XPlane
{
    public class XPlaneClient : ClientBase, IEHSIProvider, ILoadable
    {
        UdpClient sendclient;
        UdpClient srvclient;
        IPEndPoint sendiPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 49000);
        IPEndPoint localiPEndPoint;
        protected override void Start()
        {
            Observable.Start(() =>
            {
                try
                {
                    Status = "Starting";

                    sendclient = new UdpClient();

                    sendclient.Connect(sendiPEndPoint);

                    localiPEndPoint = (IPEndPoint)sendclient.Client.LocalEndPoint;

                    srvclient = new UdpClient(localiPEndPoint);

                    srvclient.BeginReceive(new AsyncCallback(receive), null);

                    Running = true;
                    Status = "Running";
                    CanStart = false;
                }
                catch (Exception ex)
                {
                    Stop();
                    Status = ex.Message;
                }
            });

        }

        void receive(IAsyncResult result)
        {
            try
            {
                byte[] values = srvclient.EndReceive(result, ref localiPEndPoint);

                ExecuteResponse(values);

                if (Running)
                    srvclient.BeginReceive(new AsyncCallback(receive), null);
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                Stop();
                Status = ex.Message;
            }
        }

        [Category("Values")]
        [ExpandableObject]
        public EHSIModel EHSI { get; } = new EHSIModel();
        IDisposable runningSubscription;

        public void StartEHSI()
        {
            Observable.Start(() =>
            {
                try
                {
                    runningSubscription = this.WhenAnyValue(x => x.Running).Subscribe(x =>
                    {
                        if (x)
                            this.InternalStartEHSI();
                        else
                            this.InternalStopEHSI();
                    });
                }
                catch (Exception ex)
                {
                    Status = ex.Message;
                }
            }, RxApp.TaskpoolScheduler);
        }

        void InternalStopEHSI()
        {
            if (Running)
            {
                byte[] packet = Encoding.ASCII.GetBytes(string.Format("RPOS{0}0{0}", (char)0));
                sendclient.Send(packet, packet.Length);

                EHSI.Running = false;
            }
        }
        void InternalStartEHSI()
        {
            try
            {
                byte[] packet = Encoding.ASCII.GetBytes(string.Format("RPOS{0}60{0}", (char)0));
                sendclient.Send(packet, packet.Length);

                EHSI.Running = true;
            }
            catch (Exception ex)
            {
                Status = ex.Message;

                Stop();
            }
        }

        public void StopEHSI()
        {
            try
            {
                InternalStopEHSI();

                runningSubscription?.Dispose();
                runningSubscription = null;
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
        }


        void ExecuteResponse(byte[] buffer)
        {
            var pos = 0;
            var header = Encoding.UTF8.GetString(buffer, pos, 4);

            buffer = buffer.Skip(5).ToArray();

            if (header == "RPOS" && runningSubscription != null)
            {
                RPOS rpos = ByteArrayToStructure<RPOS>(buffer);
                EHSI.FlightHeading = rpos.veh_psi_loc;
                EHSI.Latitude = rpos.dat_lat;
                EHSI.Longitude = rpos.dat_lon;
                EHSI.MeanAltitude = rpos.dat_ele;

                EHSI.CalculateWaypoint();
            }
        }

        public struct RPOS
        {
            public double dat_lon;
            public double dat_lat;
            public double dat_ele;
            public float y_agl_mtr;
            public float veh_the_loc;
            public float veh_psi_loc;
            public float veh_phi_loc;
            public float vx_wrl;
            public float vy_wrl;
            public float vz_wrl;
            public float Prad;
            public float Qrad;
            public float Rrad;

            public override string ToString()
            {
                return string.Concat(
                    "dat_lon:     ", dat_lat.ToString("N6"), Environment.NewLine,
                    "dat_lat:     ", dat_lat.ToString("N6"), Environment.NewLine,
                    "dat_ele:     ", dat_ele.ToString("N6"), Environment.NewLine,
                    "y_agl_mtr:   ", y_agl_mtr.ToString("N6"), Environment.NewLine,
                    "veh_the_loc: ", veh_the_loc.ToString("N6"), Environment.NewLine,
                    "veh_psi_loc: ", veh_psi_loc.ToString("N6"), Environment.NewLine,
                    "veh_phi_loc: ", veh_phi_loc.ToString("N6"), Environment.NewLine,
                    "vx_wrl:      ", vx_wrl.ToString("N6"), Environment.NewLine,
                    "vy_wrl:      ", vy_wrl.ToString("N6"), Environment.NewLine,
                    "vz_wrl:      ", vz_wrl.ToString("N6"), Environment.NewLine,
                    "Prad:        ", Prad.ToString("N6"), Environment.NewLine,
                    "Qrad:        ", Qrad.ToString("N6"), Environment.NewLine,
                    "Rrad:        ", Rrad.ToString("N6"), Environment.NewLine
                    );
            }
        }

        T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            T stuff;
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
            return stuff;
        }

        protected override void Stop()
        {
            try
            {
                Status = "Stopping";

                InternalStop();

                Status = "Stopped";
                CanStart = true;
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
        }

        void InternalStop()
        {
            Running = false;

            sendclient?.Dispose();
            srvclient?.Dispose();
        }


        protected override void InternalDispose()
        {
            InternalStop();
        }

        public void Load()
        {

            Observable.Start(() =>
            {
                try
                {
                    Status = "Loading";

                    IEnumerable<Waypoint> fixes = null;
                    IEnumerable<Waypoint> navs = null;

                    Task<IEnumerable<Waypoint>> fixresult = Task.Run(() => fixes = LoadFixes());
                    Task<IEnumerable<Waypoint>> navresult = Task.Run(() => navs = LoadNavs());

                    Task.WaitAll(fixresult, navresult);

                    Observable.Start(() =>
                    {
                        Waypoints.Clear();
                        Waypoints.AddRange(navs);
                        Waypoints.AddRange(fixes);
                    }, RxApp.MainThreadScheduler);

                    Status = "Loaded";
                }
                catch (Exception ex)
                {
                    Status = ex.Message;
                }
            }, RxApp.TaskpoolScheduler);

        }

        readonly string[] navBearingType = new string[] { "4", "5" };

        double? ParseBearing(string type, string val)
        {
            switch (type)
            {
                case "4":
                case "5":
                case "7":
                case "8":
                case "9":
                case "14":
                    return double.Parse(val, CultureInfo.InvariantCulture);
                case "6":
                case "15":
                case "16":
                    return double.Parse(val.Substring(3), CultureInfo.InvariantCulture);
            }
            return null;
        }

        WaypointType ParseNavType(string val)
        {
            switch (val)
            {
                case "2":
                    return WaypointType.NDB;
                case "3":
                    return WaypointType.VOR;
                case "4":
                case "5":
                    return WaypointType.LOC;
                case "6":
                    return WaypointType.Glideslope;
                case "7":
                    return WaypointType.BeaconOM;
                case "8":
                    return WaypointType.BeaconMM;
                case "9":
                    return WaypointType.BeaconIM;
                case "12":
                case "13":
                    return WaypointType.DME;
                case "14":
                    return WaypointType.LPV;
                case "15":
                    return WaypointType.GLS;
                case "16":
                    return WaypointType.WAAS;

            }
            throw new NotImplementedException();
        }

        const string navfilename = @"J:\X-Plane 11\Custom Data\earth_nav.dat";
        IEnumerable<Waypoint> LoadNavs()
        {
            List<Waypoint> list = new List<Waypoint>();
            using (StreamReader sr = new StreamReader(navfilename, true))
            {
                string line = sr.ReadLine();
                if (line != "I")
                    throw new FileFormatException(navfilename);
                line = sr.ReadLine();
                if (!line.StartsWith("1100"))
                    throw new FileFormatException(navfilename);
                line = sr.ReadLine();
                if (line.Trim().Length > 0)
                    throw new FileFormatException(navfilename);

                while ((line = sr.ReadLine()) != null && line != "99")
                {
                    string[] values = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    list.Add(new Waypoint(
                        ParseNavType(values[0]), string.Join(" ", values.Skip(9)),
                        double.Parse(values[1], CultureInfo.InvariantCulture),
                        double.Parse(values[2], CultureInfo.InvariantCulture),
                        double.Parse(values[3], CultureInfo.InvariantCulture),
                        ParseBearing(values[0], values[6]), values[9], values[8]));
                }
            }
            return list;
        }

        const string fixfilename = @"J:\X-Plane 11\Custom Data\earth_fix.dat";
        IEnumerable<Waypoint> LoadFixes()
        {
            List<Waypoint> list = new List<Waypoint>();
            using (StreamReader sr = new StreamReader(fixfilename, true))
            {
                string line = sr.ReadLine();
                if (line != "I")
                    throw new FileFormatException(fixfilename);
                line = sr.ReadLine();
                if (!line.StartsWith("1101"))
                    throw new FileFormatException(fixfilename);
                line = sr.ReadLine();
                if (line.Trim().Length > 0)
                    throw new FileFormatException(fixfilename);

                while ((line = sr.ReadLine()) != null && line != "99")
                {
                    string[] values = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    list.Add(new Waypoint(
                        WaypointType.Fix, values[2],
                        double.Parse(values[0], CultureInfo.InvariantCulture),
                        double.Parse(values[1], CultureInfo.InvariantCulture),
                        0, null, values[4], values[3]));
                }
            }
            return list;
        }

    }
}
