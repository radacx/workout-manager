using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WorkoutManager.App.Converters
{
    internal class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case bool boolValue:

                    var result = boolValue ? Visibility.Visible : Visibility.Collapsed;

                    return result;
                default:

                    return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}