using System;
using System.Globalization;
using System.Windows.Data;

namespace UMFDExtractor.Converters
{
    public class DistanceConverter : IValueConverter
    {
        private static readonly string[] UNITS = { "m", "km", "Mm", "Gm", "Tm" };
        private static readonly string[] FORMATS = { "###0", "###.0", "##.00", "#0.000" };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                int unit = 0;
                double distance = (double)value;
                while (unit < 4 && distance >= 1000.0)
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
                        if (param.StartsWith("U"))
                            return UNITS[unit];
                        if (param.StartsWith("V"))
                            return distance.ToString(FORMATS[n], CultureInfo.InvariantCulture);
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
