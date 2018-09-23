using System;
using System.Globalization;
using System.Windows.Data;

namespace WorkoutManager.App.Converters
{
    public class IsTextEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                return text.Trim().Length == 0;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}