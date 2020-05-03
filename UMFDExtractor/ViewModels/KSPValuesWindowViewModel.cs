using System;
using UMFDExtractor.Models.KSP;

namespace UMFDExtractor.ViewModels
{
    [Obsolete]
    public class KSPValuesWindowViewModel : ViewModelBase
    {
        public KSPClient Client { get; }
        public KSPValues Values { get; }

        public KSPValuesWindowViewModel()
        {
            Client = new KSPClient();
            Values = new KSPValues(Client);
        }
        public KSPValuesWindowViewModel(KSPClient client)
        {
            Client = client;
            Values = new KSPValues(client);
        }

        public void Close()
        {
            Values.Dispose();
        }

    }
}
