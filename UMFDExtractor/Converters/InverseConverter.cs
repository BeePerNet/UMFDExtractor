using System;
using System.Globalization;
using System.Windows.Data;

namespace UMFDExtractor.Converters
{
    public class InverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
                return (double)value * -1;
            if (value is float)
                return (float)value * -1;
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

