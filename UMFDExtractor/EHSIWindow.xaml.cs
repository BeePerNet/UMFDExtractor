using UMFDExtractor.ViewModels;
using ReactiveUI;
using UMFDExtractor.Models;

namespace UMFDExtractor
{
    /// <summary>
    /// Logique d'interaction pour EHSIWindow.xaml
    /// </summary>
    public partial class EHSIWindow : ReactiveWindow<EHSIWindowViewModel>
    {
        public EHSIWindow()
        {
            InitializeComponent();
        }

        PropertiesWindow propertiesWindow;

        private void ReactiveWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            propertiesWindow?.Close();
            (this.DataContext as EHSIWindowViewModel)?.Close();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (propertiesWindow == null || !propertiesWindow.IsActive)
            {
                propertiesWindow = new PropertiesWindow();
                propertiesWindow.DataContext = ((this.DataContext as EHSIWindowViewModel).Client as IEHSIProvider).EHSI;
                propertiesWindow.Show();
            }
        }
    }
}
