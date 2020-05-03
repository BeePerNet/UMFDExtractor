using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UMFDExtractor.ViewModels;

namespace UMFDExtractor.Windows
{
    /// <summary>
    /// Logique d'interaction pour WaypointsWindow.xaml
    /// </summary>
    public partial class WaypointsWindow : ReactiveWindow<MainViewModel>
    {
        public WaypointsWindow()
        {
            InitializeComponent();
        }

        public WaypointsWindow(MainViewModel vm):this()
        {
            this.DataContext = vm;
        }


        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;
            Process.Start(link.NavigateUri.AbsoluteUri);
        }
    }
}
