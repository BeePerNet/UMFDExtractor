using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using KSPDataExtractor.ViewModels;
using System;

namespace KSPDataExtractor.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
            MainWindowViewModel vm = this.DataContext as MainWindowViewModel;

            vm.Close();
        }


    }

}
