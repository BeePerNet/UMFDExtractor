using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using UMFDExtractor.Models;
using UMFDExtractor.ViewModels;

namespace UMFDExtractor.Windows
{
    /// <summary>
    /// Logique d'interaction pour EHSIWindow.xaml
    /// </summary>
    public partial class EHSIWindow : ReactiveWindow<MainViewModel>
    {
        public EHSIWindow()
        {
            InitializeComponent();
        }

        public EHSIWindow(MainViewModel vm) : this()
        {
            this.DataContext = vm;

            vm.WhenAnyValue(x => x.Client).Subscribe(x => (x as IEHSIProvider).StartEHSI());
        }

        PropertiesWindow propertiesWindow;

        private void ReactiveWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            propertiesWindow?.Close();

            ((this.DataContext as MainViewModel).Client as IEHSIProvider).StopEHSI();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (propertiesWindow == null || !propertiesWindow.IsActive)
            {
                propertiesWindow = new PropertiesWindow();
                propertiesWindow.SetDisposable((this.DataContext as MainViewModel).WhenAnyValue(x => x.Client).Subscribe(x => propertiesWindow.DataContext = (x as IEHSIProvider)?.EHSI));
                propertiesWindow.Show();
            }
        }
    }
}
