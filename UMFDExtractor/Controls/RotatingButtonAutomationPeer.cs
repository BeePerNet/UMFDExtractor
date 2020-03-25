using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

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
            //owner.AutomationButtonBaseClick();
        }
    }
}