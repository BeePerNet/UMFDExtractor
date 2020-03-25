using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace KSPDataExtractor.Views
{
    public class EHSIView : UserControl
    {
        public EHSIView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
