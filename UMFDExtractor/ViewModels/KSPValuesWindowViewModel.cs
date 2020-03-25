using UMFDExtractor.Models.KSP;

namespace UMFDExtractor.ViewModels
{
    public class KSPValuesWindowViewModel : ViewModelBase
    {
        public KSPClient Client { get; }
        public KSPValues Values { get; }

        public KSPValuesWindowViewModel()
        {
            Client = new KSPClient();
            Values = new KSPValues();
        }
        public KSPValuesWindowViewModel(KSPClient client)
        {
            Client = client;
            Values = client.KSPValues;
        }

        public void Close()
        {
            Values.Dispose();
        }

    }
}
