using UMFDExtractor.ViewModels;
using ReactiveUI;
using System.ComponentModel;
using System.Windows;
using UMFDExtractor.Windows;

namespace UMFDExtractor
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainViewModel(false);
        }

        private void ReactiveWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                if (window != this)
                {
                    window.Close();
                }
            }
        }

    }
}
