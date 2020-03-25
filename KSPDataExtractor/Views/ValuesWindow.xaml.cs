using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using KSPDataExtractor.ViewModels;
using System;

namespace KSPDataExtractor.Views
{
    public class ValuesWindow : Window
    {
        public ValuesWindow()
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
            ValuesWindowViewModel vm = this.DataContext as ValuesWindowViewModel;

            vm.Close();
        }
    }
}
