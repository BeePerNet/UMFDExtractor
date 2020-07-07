using ReactiveUI;
using System;
using System.ComponentModel;

namespace UMFDExtractor.Models
{

    public class ReactiveObjectBase: ReactiveObject
    {
        [Browsable(false)]
        public new IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changing { get { return base.Changing; } }
        [Browsable(false)]
        public new IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changed { get { return base.Changed; } }
        [Browsable(false)]
        public new IObservable<Exception> ThrownExceptions { get { return base.ThrownExceptions; } }
    }
}
