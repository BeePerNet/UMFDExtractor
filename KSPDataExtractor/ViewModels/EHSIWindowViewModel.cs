using KSPDataExtractor.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KSPDataExtractor.ViewModels
{
    public class EHSIWindowViewModel : ViewModelBase
    {
        KSPClient Client { get; }
        EHSIViewModel EHSI { get; }

        public EHSIWindowViewModel()
        {
            Client = new KSPClient();
            EHSI = new EHSIViewModel();
        }
        public EHSIWindowViewModel(KSPClient client)
        {
            Client = client;
            EHSI = new EHSIViewModel(client);
        }

        public void Close()
        {
            EHSI.Dispose();
        }
    }
}
