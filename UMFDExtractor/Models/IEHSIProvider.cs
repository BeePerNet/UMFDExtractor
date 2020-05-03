namespace UMFDExtractor.Models
{
    public interface IEHSIProvider : IClient
    {
        EHSIModel EHSI { get; }
        void StartEHSI();

        void StopEHSI();
    }
}
