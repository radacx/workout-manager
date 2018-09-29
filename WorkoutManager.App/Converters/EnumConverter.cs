using System;
using System.Globalization;
using System.Windows.Data;
using WorkoutManager.Contract.Extensions;

namespace WorkoutManager.App.Converters
{
    internal class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum enumValue)
            {
                return enumValue.GetDescription();
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}