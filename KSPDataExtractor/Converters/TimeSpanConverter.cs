using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace KSPDataExtractor.Converters
{
    public class TimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                double time = (double)value;
                string h = string.Empty;
                string m = string.Empty;
                if (time > 3600)
                {
                    h = string.Format("{0:0}h ", Math.Truncate(time / 3600));
                    time = time % 3600;
                }
                if (time > 60)
                {
                    m = string.Format("{0:0}m ", Math.Truncate(time / 60));
                    time = time % 60;
                }
                return string.Format("{0}{1}{2:00.0} s", h, m, time);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
