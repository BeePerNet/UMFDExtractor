using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Threading;

namespace UMFDExtractor.Controls
{
    public class RotatingButtonAutomationPeer : ButtonBaseAutomationPeer, IInvokeProvider
    {
        ///
        public RotatingButtonAutomationPeer(RotatingButton owner) : base(owner)
        { }

        ///
        override protected string GetClassNameCore()
        {
            return "RepeatButton";
        }

        ///
        override protected AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Button;
        }

        /// 
        override public object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.Invoke)
            {
                return this;
            }
            else
            {
                return base.GetPattern(patternInterface);
            }
        }

        void IInvokeProvider.Invoke()
        {
            if (!IsEnabled())
                throw new ElementNotEnabledException();

            //RotatingButton owner = (RotatingButton)Owner;
            //owner.Click();
            //owner.AutomationButtonBaseClick();

            // Async call of click event
            // In ClickHandler opens a dialog and suspend the execution we don't want to block this thread
            Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate (object param)
            {
                ((RotatingButton)Owner).AutomationButtonBaseClick();
                return null;
            }), null);
        }
    }
}