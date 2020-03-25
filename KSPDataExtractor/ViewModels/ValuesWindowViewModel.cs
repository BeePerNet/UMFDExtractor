using KSPDataExtractor.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KSPDataExtractor.ViewModels
{
    public class ValuesWindowViewModel : ViewModelBase
    {
        KSPClient Client { get; }
        ValuesViewModel Values { get; }

        public ValuesWindowViewModel()
        {
            Client = new KSPClient();
            Values = new ValuesViewModel();
        }
        public ValuesWindowViewModel(KSPClient client)
        {
            Client = client;
            Values = new ValuesViewModel(client);
        }

        public void Close()
        {
            Values.Dispose();
        }

    }
}
