using KRPC.Client;
using KRPC.Client.Services.SpaceCenter;
using KSPDataExtractor.Models;
using KSPDataExtractor.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace KSPDataExtractor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public KSPClient Client { get; } = new KSPClient();
        ValuesWindow valuesWindow;
        EHSIWindow ehsiWindow;

        public MainWindowViewModel()
        {
            OpenValuesWindowCommand = ReactiveCommand.Create(OpenValuesWindow);
            OpenEHSIWindowCommand = ReactiveCommand.Create(OpenEHSIWindow);
        }

        public ReactiveCommand<Unit, Unit> OpenValuesWindowCommand { get; }
                                           
        public ReactiveCommand<Unit, Unit> OpenEHSIWindowCommand { get; }

        void OpenValuesWindow()
        {
            if (valuesWindow == null || !valuesWindow.IsActive)
            {
                valuesWindow = new ValuesWindow();
                valuesWindow.DataContext = new ValuesWindowViewModel(Client);
                valuesWindow.Show();
            }
        }

        void OpenEHSIWindow()
        {
            if (ehsiWindow == null || !ehsiWindow.IsActive)
            {
                ehsiWindow = new EHSIWindow();
                ehsiWindow.DataContext = new EHSIWindowViewModel(Client);
                ehsiWindow.Show();
            }
        }
        public void Close()
        {
            valuesWindow?.Close();
            ehsiWindow?.Close();

            Client.Dispose();
        }
    }
}
