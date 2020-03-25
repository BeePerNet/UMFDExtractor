using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UMFDExtractor.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool visible = false;

            if (value is bool)
                visible = (bool)value;
            else if (value != null)
            {
                if (IsNumericType(value))
                    visible = System.Convert.ToBoolean(value);
                else
                    visible = true;
            }
            if (parameter != null)
                visible = !visible;
            if (targetType == typeof(Visibility))
                return visible ? Visibility.Visible : Visibility.Collapsed;
            else if (targetType == typeof(bool))
                return visible;
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static bool IsNumericType(object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}
