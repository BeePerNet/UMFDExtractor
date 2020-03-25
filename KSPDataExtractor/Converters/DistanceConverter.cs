using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace KSPDataExtractor.Converters
{
    public class DistanceConverter : IValueConverter
    {
        private static string[] UNITS = { "m", "km", "Mm", "Gm", "Tm" };
        private static string[] FORMATS = { "###0", "###.0", "##.00", "#0.000" };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                int unit = 0;
                double distance = (double)value;
                while (unit < 4 && distance >= 10000.0)
                {
                    distance /= 1000.0;
                    unit++;
                }

                int n = 3;
                if (distance > 9)
                    n = 3 - (int)Math.Log10(distance);

                if (parameter != null)
                {
                    string param = parameter.ToString();
                    if (!string.IsNullOrWhiteSpace(param))
                    {
                        param = param.ToUpper();
                        if (param.StartsWith('U'))
                            return UNITS[unit];
                        if (param.StartsWith('V'))
                            return distance.ToString(FORMATS[n]);
                    }
                }
                return distance.ToString(string.Concat(FORMATS[n], " ", UNITS[unit]));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
