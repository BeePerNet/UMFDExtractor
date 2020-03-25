using UMFDExtractor.ViewModels;
using ReactiveUI;
using System.ComponentModel;

namespace UMFDExtractor
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainWindowViewModel(false);
        }

        private void ReactiveWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (this.DataContext as MainWindowViewModel)?.Close();
            propertiesWindow?.Close();
        }

        PropertiesWindow propertiesWindow;
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            propertiesWindow = new PropertiesWindow();
            propertiesWindow.DataContext = this.DataContext;
            propertiesWindow.Show();
        }
    }
}
