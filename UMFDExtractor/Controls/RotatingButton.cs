using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace UMFDExtractor.Controls
{
    /// <summary>
    /// Suivez les étapes 1a ou 1b puis 2 pour utiliser ce contrôle personnalisé dans un fichier XAML.
    ///
    /// Étape 1a) Utilisation de ce contrôle personnalisé dans un fichier XAML qui existe dans le projet actif.
    /// Ajoutez cet attribut XmlNamespace à l'élément racine du fichier de balisage où il doit 
    /// être utilisé :
    ///
    ///     xmlns:MyNamespace="clr-namespace:UMFDExtractor.Controls"
    ///
    ///
    /// Étape 1b) Utilisation de ce contrôle personnalisé dans un fichier XAML qui existe dans un autre projet.
    /// Ajoutez cet attribut XmlNamespace à l'élément racine du fichier de balisage où il doit 
    /// être utilisé :
    ///
    ///     xmlns:MyNamespace="clr-namespace:UMFDExtractor.Controls;assembly=UMFDExtractor.Controls"
    ///
    /// Vous devrez également ajouter une référence du projet contenant le fichier XAML
    /// à ce projet et regénérer pour éviter des erreurs de compilation :
    ///
    ///     Cliquez avec le bouton droit sur le projet cible dans l'Explorateur de solutions, puis sur
    ///     "Ajouter une référence"->"Projets"->[Recherchez et sélectionnez ce projet]
    ///
    ///
    /// Étape 2)
    /// Utilisez à présent votre contrôle dans le fichier XAML.
    ///
    ///     <MyNamespace:RotatingButton/>
    ///
    /// </summary>
    public class RotatingButton : ButtonBase
    {
        static RotatingButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RotatingButton), new FrameworkPropertyMetadata(typeof(RotatingButton)));

            //FocusableProperty.OverrideMetadata(typeof(RotatingButton), new UIPropertyMetadata(true));
            //_dType = DependencyObjectType.FromSystemTypeInternal(typeof(RepeatButton));
            ClickModeProperty.OverrideMetadata(typeof(RotatingButton), new FrameworkPropertyMetadata(ClickMode.Press));

            IsEnabledProperty.OverrideMetadata(typeof(RotatingButton), new FrameworkPropertyMetadata(true));
        }

        public static readonly DependencyProperty RotationProperty = DependencyProperty.Register(nameof(Rotation), typeof(double), typeof(RotatingButton), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(nameof(MinValue), typeof(double), typeof(RotatingButton), new PropertyMetadata(0d));
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(nameof(MaxValue), typeof(double), typeof(RotatingButton), new PropertyMetadata(360d));

        public RotatingButton() : base() { }

        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        [Bindable(true), Category("Behavior")]
        public double Rotation
        {
            get { return (double)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
        }

        #region Dependencies and Events

        /// <summary>
        ///     The Property for the Delay property.
        ///     Flags:              Can be used in style rules
        ///     Default Value:      Depend on SPI_GETKEYBOARDDELAY from SystemMetrics
        /// </summary>
        public static readonly DependencyProperty DelayProperty
            = DependencyProperty.Register("Delay", typeof(int), typeof(RotatingButton),
                                          new FrameworkPropertyMetadata(GetKeyboardDelay()),
                                          new ValidateValueCallback(IsDelayValid));

        /// <summary>
        ///     Specifies the amount of time, in milliseconds, to wait before repeating begins.
        /// Must be non-negative
        /// </summary>
        [Bindable(true), Category("Behavior")]
        public int Delay
        {
            get
            {
                return (int)GetValue(DelayProperty);
            }
            set
            {
                SetValue(DelayProperty, value);
            }
        }

        /// <summary>
        ///     The Property for the Interval property.
        ///     Flags:              Can be used in style rules
        ///     Default Value:      Depend on SPI_GETKEYBOARDSPEED from SystemMetrics
        /// </summary>
        public static readonly DependencyProperty IntervalProperty
            = DependencyProperty.Register("Interval", typeof(int), typeof(RotatingButton),
                                          new FrameworkPropertyMetadata(GetKeyboardSpeed()),
                                          new ValidateValueCallback(IsIntervalValid));

        /// <summary>
        ///     Specifies the amount of time, in milliseconds, between repeats once repeating starts.
        /// Must be non-negative
        /// </summary>
        [Bindable(true), Category("Behavior")]
        public int Interval
        {
            get
            {
                return (int)GetValue(IntervalProperty);
            }
            set
            {
                SetValue(IntervalProperty, value);
            }
        }

        #endregion Dependencies and Events

        #region Private helpers

        private static bool IsDelayValid(object value) { return ((int)value) >= 0; }
        private static bool IsIntervalValid(object value) { return ((int)value) > 0; }

        /// <summary>
        /// Starts a _timer ticking
        /// </summary>
        private void StartTimer()
        {
            if (_timer == null)
            {
                _timer = new DispatcherTimer();
                _timer.Tick += new EventHandler(OnTimeout);
            }
            else if (_timer.IsEnabled)
                return;

            _timer.Interval = TimeSpan.FromMilliseconds(Delay);
            _timer.Start();
        }

        /// <summary>
        /// Stops a _timer that has already started
        /// </summary>
        private void StopTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
            }
        }

        /// <summary>
        /// This is the handler for when the repeat _timer expires. All we do
        /// is invoke a click.
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void OnTimeout(object sender, EventArgs e)
        {
            TimeSpan interval = TimeSpan.FromMilliseconds(Interval);
            if (_timer.Interval != interval)
                _timer.Interval = interval;

            //if (IsPressed)
            {
                OnClick();
            }
        }

        /// <summary>
        /// Retrieves the keyboard repeat-delay setting, which is a value in the range from 0
        /// (approximately 250 ms delay) through 3 (approximately 1 second delay).
        /// The actual delay associated with each value may vary depending on the hardware.
        /// </summary>
        /// <returns></returns>
        internal static int GetKeyboardDelay()
        {
            int delay = SystemParameters.KeyboardDelay;
            // SPI_GETKEYBOARDDELAY 0,1,2,3 correspond to 250,500,750,1000ms
            if (delay < 0 || delay > 3)
                delay = 0;
            return (delay + 1) * 250;
        }

        /// <summary>
        /// Retrieves the keyboard repeat-speed setting, which is a value in the range from 0
        /// (approximately 2.5 repetitions per second) through 31 (approximately 30 repetitions per second).
        /// The actual repeat rates are hardware-dependent and may vary from a linear scale by as much as 20%
        /// </summary>
        /// <returns></returns>
        internal static int GetKeyboardSpeed()
        {
            int speed = SystemParameters.KeyboardSpeed;
            // SPI_GETKEYBOARDSPEED 0,...,31 correspond to 1000/2.5=400,...,1000/30 ms
            /*if (speed < 0 || speed > 31)
                speed = 31;
            return (31 - speed) * (400 - 1000 / 30) / 31 + 1000 / 30;*/
            return 8;
        }

        #endregion Private helpers

        #region Override methods

        /// <summary>
        /// Creates AutomationPeer (<see cref="UIElement.OnCreateAutomationPeer"/>)
        /// </summary>
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new RotatingButtonAutomationPeer(this);
        }

        /// <summary>
        /// This method is called when button is clicked via IInvokeProvider.
        /// </summary>
        internal void AutomationButtonBaseClick()
        {
            OnClick();
        }

        /// <summary>
        /// Raises InvokedAutomationEvent and call the base method to raise the Click event
        /// </summary>
        /// <ExternalAPI/>
        protected override void OnClick()
        {
            if (AutomationPeer.ListenerExists(AutomationEvents.InvokePatternOnInvoked))
            {
                AutomationPeer peer = UIElementAutomationPeer.CreatePeerForElement(this);
                if (peer != null)
                    peer.RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
            }

            if (IsEnabled)
            {
                base.OnClick();

                double rotation = Rotation;
                double step = 1;
                if (Keyboard.Modifiers == ModifierKeys.Control)
                    step = 0.1;
                rotation += (increment ? 1 : -1) * step;
                if (rotation < MinValue)
                {
                    rotation = MinValue - rotation;
                    Rotation = MaxValue - rotation;
                }
                else if (rotation >= MaxValue)
                {
                    Rotation = rotation - MaxValue + MinValue;
                }
                else
                {
                    Rotation = rotation;
                }
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            if (!e.Handled && IsEnabled)
            {
                if (e.Delta != 0)
                {
                    double rotation = Rotation;
                    double step = 1;
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                        step = 0.1;
                    rotation += (e.Delta > 0 ? 1 : -1) * step;
                    if (rotation < MinValue)
                    {
                        rotation = MinValue - rotation;
                        Rotation = MaxValue - rotation;
                    }
                    else if (rotation >= MaxValue)
                    {
                        Rotation = rotation - MaxValue + MinValue;
                    }
                    else
                    {
                        Rotation = rotation;
                    }

                    e.Handled = true;
                }
            }
        }


        bool increment = false;


        /// <summary>
        /// This is the method that responds to the MouseButtonEvent event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            increment = false;

            base.OnMouseLeftButtonDown(e);

            //if (IsPressed && (ClickMode != ClickMode.Hover))
            if (IsEnabled)
            {
                StartTimer();
            }
        }

        /// <summary>
        /// This is the method that responds to the MouseButtonEvent event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            //if (ClickMode != ClickMode.Hover)
            {
                StopTimer();
            }
        }

        /// <summary>
        /// This is the method that responds to the MouseButtonEvent event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            increment = true;

            base.OnMouseRightButtonDown(e);

            OnClick();
            //if (IsPressed && (ClickMode != ClickMode.Hover))
            if (IsEnabled)
            {
                StartTimer();
            }
        }

        /// <summary>
        /// This is the method that responds to the MouseButtonEvent event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonUp(e);

            //if (ClickMode != ClickMode.Hover)
            {
                StopTimer();
            }
        }
        /// <summary>
        ///     Called when this element loses mouse capture.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            base.OnLostMouseCapture(e);
            StopTimer();
        }

        /*/// <summary>
        ///     An event reporting the mouse entered this element.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (HandleIsMouseOverChanged())
            {
                e.Handled = true;
            }
        }*/

        /// <summary>
        ///     An event reporting the mouse left this element.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            StopTimer();
            /*if (HandleIsMouseOverChanged())
            {
                e.Handled = true;
            }*/
        }
        /*
        /// <summary>
        ///     An event reporting that the IsMouseOver property changed.
        /// </summary>
        private bool HandleIsMouseOverChanged()
        {
            if (ClickMode == ClickMode.Hover)
            {
                if (IsMouseOver)
                {
                    StartTimer();
                }
                else
                {
                    StopTimer();
                }

                return true;
            }

            return false;
        }*/

        /// <summary>
        /// This is the method that responds to the KeyDown event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if ((e.Key == Key.Space) && (ClickMode != ClickMode.Hover))
            {
                StartTimer();
            }
        }

        /// <summary>
        /// This is the method that responds to the KeyUp event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if ((e.Key == Key.Space) && (ClickMode != ClickMode.Hover))
            {
                StopTimer();
            }
            base.OnKeyUp(e);
        }

        /*
        //  This property
        //  1. Finds the correct initial size for the _effectiveValues store on the current DependencyObject
        //  2. This is a performance optimization
        //
        internal override int EffectiveValuesInitialSize
        {
            get { return 28; }
        }*/

        #endregion

        #region Data

        private DispatcherTimer _timer;

        #endregion

        /*#region DTypeThemeStyleKey

        // Returns the DependencyObjectType for the registered ThemeStyleKey's default 
        // value. Controls will override this method to return approriate types.
        internal override DependencyObjectType DTypeThemeStyleKey
        {
            get { return _dType; }
        }

        private static DependencyObjectType _dType;

        #endregion DTypeThemeStyleKey*/
    }
}


