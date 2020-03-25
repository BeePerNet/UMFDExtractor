using System;
using System.Globalization;
using System.Windows.Data;

namespace UMFDExtractor.Converters
{
    public class DividerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
                return (double)value * 5;
            if (value is int)
                return (int)value * 5;
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
