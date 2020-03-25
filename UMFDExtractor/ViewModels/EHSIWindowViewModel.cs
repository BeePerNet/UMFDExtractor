using ReactiveUI;
using UMFDExtractor.Models;

namespace UMFDExtractor.ViewModels
{
    public class EHSIWindowViewModel : ViewModelBase
    {
        public IEHSIProvider client;
        public IEHSIProvider Client
        {
            get => client;
            private set => this.RaiseAndSetIfChanged(ref client, value);            
        }

        public void SetClient(IEHSIProvider provider)
        {
            Client = provider;
            Client.StartEHSI();
        }

        public EHSIWindowViewModel()
        {
            Client = new Models.XPlane.XPlaneClient();
        }
        public EHSIWindowViewModel(IEHSIProvider client)
        {
            Client = client;
            Client.StartEHSI();
        }

        public void Close()
        {
            Client.StopEHSI();
        }
    }
}
