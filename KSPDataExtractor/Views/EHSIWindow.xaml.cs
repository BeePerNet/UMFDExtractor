using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using KSPDataExtractor.ViewModels;
using System;

namespace KSPDataExtractor.Views
{
    public class EHSIWindow : Window
    {
        public EHSIWindow()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void Close(object source, EventArgs e)
        {
            EHSIWindowViewModel vm = this.DataContext as EHSIWindowViewModel;

            vm.Close();
        }
    }
}
