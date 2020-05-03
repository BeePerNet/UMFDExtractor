using ReactiveUI;
using System;
using System.Collections.Generic;
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
    /// Logique d'interaction pour PropertiesWindow.xaml
    /// </summary>
    public partial class PropertiesWindow : Window
    {
        public PropertiesWindow()
        {
            InitializeComponent();
        }

        IDisposable disposable;

        public void SetDisposable(IDisposable disposable)
        {
            this.disposable = disposable;
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            disposable?.Dispose();
        }
    }
}
