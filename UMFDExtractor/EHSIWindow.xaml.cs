using UMFDExtractor.ViewModels;
using ReactiveUI;

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

        private void ReactiveWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (this.DataContext as EHSIWindowViewModel)?.Close();
        }
    }
}
