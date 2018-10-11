using System;
using System.Globalization;
using System.Windows.Data;
using WorkoutManager.Service.Services;

namespace WorkoutManager.App.Converters
{
    internal class ProgressResultConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ProgressResult result))
            {
                return null;
            }

            return $"{result.Date.ToShortDateString()}: {result.Volume}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}