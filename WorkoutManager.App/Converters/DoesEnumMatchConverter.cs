using System;
using System.Globalization;
using System.Windows.Data;

namespace WorkoutManager.App.Converters
{
    internal class DoesEnumMatchConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum enumValue
                && parameter is Enum enumParameter
                && enumValue.GetType() == enumParameter.GetType())
            {
                return Equals(enumValue, enumParameter);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue
                && parameter is Enum enumParameter
                && boolValue)
            {
                return enumParameter;
            }

            return null;
        }
    }
}