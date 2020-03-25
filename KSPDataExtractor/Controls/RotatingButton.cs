using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Threading;
using System;
using System.Reactive.Linq;

namespace KSPDataExtractor.Controls
{
    public class RotatingButton : Spinner
    {
        static RotatingButton()
        {
            FocusableProperty.OverrideDefaultValue(typeof(RotatingButton), true);
        }



        private double _Rotation = 0;

        public static readonly DirectProperty<RotatingButton, double> RotationProperty =
                AvaloniaProperty.RegisterDirect<RotatingButton, double>(
                    nameof(Rotation),
                    o => o.Rotation,
                    (o, v) => o.Rotation = v,
                    unsetValue: 0d,
                    defaultBindingMode: BindingMode.TwoWay);



        public static readonly StyledProperty<int> MinValueProperty =
                    AvaloniaProperty.Register<RotatingButton, int>(nameof(MinValue), 0);

        public static readonly StyledProperty<int> MaxValueProperty =
                    AvaloniaProperty.Register<RotatingButton, int>(nameof(MaxValue), 360);

        public int MinValue
        {
            get { return GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        public int MaxValue
        {
            get { return GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public double Rotation
        {
            get => _Rotation;
            set
            {
                SetAndRaise(RotationProperty, ref _Rotation, value);
            }
        }

        bool advance = true;
        bool pressed = false;
        bool running = false;

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (!e.Handled &&!running && !pressed)
            {
                if (e.MouseButton == MouseButton.Left || e.MouseButton == MouseButton.Right)
                {
                    running = true;
                    int minvalue = MinValue;
                    int maxvalue = MaxValue;

                    pressed = true;
                    advance = e.MouseButton == MouseButton.Right;

                    Observable.Start(() =>
                    {
                        double rotation = Rotation + (advance ? 1 : -1);

                        if (rotation < minvalue)
                        {
                            rotation = minvalue - rotation;
                            rotation = maxvalue - rotation;
                        }
                        else if (rotation >= maxvalue)
                        {
                            rotation = rotation - maxvalue + minvalue;
                        }

                        Dispatcher.UIThread.Post(() => Rotation = rotation);

                        Observable.Timer(TimeSpan.FromMilliseconds(300)).Wait();

                        while (pressed)
                        {
                            rotation = rotation + (advance ? 1 : -1);

                            if (rotation < minvalue)
                            {
                                rotation = minvalue - rotation;
                                rotation = maxvalue - rotation;
                            }
                            else if (rotation >= maxvalue)
                            {
                                rotation = rotation - maxvalue + minvalue;
                            }

                            Dispatcher.UIThread.Post(() => Rotation = rotation);

                            Observable.Timer(TimeSpan.FromMilliseconds(10)).Wait();
                        }

                        running = false;
                    });
                }
            }
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);
            if (!e.Handled)
            {
                pressed = false;
            }
        }

        /// <inheritdoc />
        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            base.OnPointerWheelChanged(e);
            if (!e.Handled)
            {
                if (e.Delta.Y != 0)
                {
                    var spinnerEventArgs = new SpinEventArgs(SpinEvent, (e.Delta.Y < 0) ? SpinDirection.Decrease : SpinDirection.Increase, true);
                    OnSpin(spinnerEventArgs);


                    double rotation = Rotation;
                    rotation += e.Delta.Y;
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

                    e.Handled = spinnerEventArgs.Handled;
                }
            }
        }


    }
}
