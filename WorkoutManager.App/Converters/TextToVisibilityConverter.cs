using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WorkoutManager.App.Converters
{
    internal class TextToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value) {
                case null:

                    return Visibility.Collapsed;
                case string text:

                    return string.IsNullOrWhiteSpace(text) ? Visibility.Collapsed: Visibility.Visible;
                default:

                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}