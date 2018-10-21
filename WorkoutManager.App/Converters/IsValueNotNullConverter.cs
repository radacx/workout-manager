using System;
using System.Globalization;
using System.Windows.Data;

namespace WorkoutManager.App.Converters
{
    internal class IsValueNotNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value != null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}