using ReactiveUI;
using System;
using System.Reactive;

namespace UMFDExtractor.Models
{
    public interface IClient: IDisposable
    {

        ReactiveCommand<Unit, Unit> StartCommand { get; }
        ReactiveCommand<Unit, Unit> StopCommand { get; }
        bool Running { get; }

        string Status { get; }
    }
}
