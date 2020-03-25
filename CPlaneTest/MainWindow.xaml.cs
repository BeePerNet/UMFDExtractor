using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XPlaneTest
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        UdpClient sendclient;
        UdpClient srvclient;
        IPEndPoint sendiPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 49000);
        IPEndPoint localiPEndPoint;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartBtn.IsEnabled = false;
            try
            {
                sendclient = new UdpClient();

                sendclient.Connect(sendiPEndPoint);

                localiPEndPoint = (IPEndPoint)sendclient.Client.LocalEndPoint;

                srvclient = new UdpClient(localiPEndPoint);

                srvclient.BeginReceive(new AsyncCallback(receive), null);

                byte[] packet = Encoding.ASCII.GetBytes(string.Format("RPOS{0}60{0}", (char)0));
                sendclient.Send(packet, packet.Length);
            }
            catch (Exception ex)
            {
                StartBtn.IsEnabled = true;
                Resultats.AppendText(ex.Message);
                Resultats.AppendText(Environment.NewLine);
            }
        }

        void receive(IAsyncResult result)
        {
            try
            {
                byte[] values = srvclient.EndReceive(result, ref localiPEndPoint);

                var test = ParseResponse(values);

                Dispatcher.Invoke(() =>
                {
                    Resultats.AppendText(Encoding.ASCII.GetString(values.Take(4).ToArray()));
                    Resultats.AppendText(" : ");
                    Resultats.AppendText(BitConverter.ToString(values));
                    Resultats.AppendText(Environment.NewLine);
                    Resultats.AppendText(test);
                    Resultats.AppendText(Environment.NewLine);
                });
                srvclient.BeginReceive(new AsyncCallback(receive), null);
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    Resultats.AppendText(ex.Message);
                    Resultats.AppendText(Environment.NewLine);
                });
            }
        }

        private string ParseResponse(byte[] buffer)
        {
            var pos = 0;
            var header = Encoding.UTF8.GetString(buffer, pos, 4);

            buffer = buffer.Skip(5).ToArray();

            if (header == "RPOS") // Ignore other messages
            {
                RPOS rpos = ByteArrayToStructure<RPOS>(buffer);

                return rpos.ToString();
            }
            return null;
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
    }
}
