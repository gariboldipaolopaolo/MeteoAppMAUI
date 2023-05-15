using System;
using System.Globalization;

namespace MeteoApp
{
    public class LongToDateTimeConverter : IValueConverter
    {
        private readonly DateTime _time = new(1970, 1, 1, 0, 0, 0, 0);
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long dateTime = (long)value;
            DateTime convertedDateTime = _time.AddSeconds(dateTime).ToLocalTime();
            return $"{convertedDateTime}";
        }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
