using System;
using System.Globalization;
using System.Windows.Data;

namespace WorkoutManager.App.Converters
{
    internal class IsTextNotEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                return !string.IsNullOrWhiteSpace(text);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}