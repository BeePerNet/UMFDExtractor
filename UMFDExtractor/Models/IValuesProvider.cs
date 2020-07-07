using System;

namespace UMFDExtractor.Models
{
    public interface IValuesProvider
    {
        IObservable<ReactiveObjectBase> Values { get; }

        void StartValues();

        void StopValues();
    }
}
