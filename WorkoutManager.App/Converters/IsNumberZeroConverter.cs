using System;
using System.Globalization;
using System.Windows.Data;
using WorkoutManager.App.Structures;

namespace WorkoutManager.App.Converters
{
    internal class IsNumberZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case int number:

                    var param = ConverterUtils.GetConverterParam(parameter);

                    return param == ConverterParam.Negation ? number != 0 : number == 0;
                default:

                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}