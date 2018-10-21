using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace WorkoutManager.App.Converters
{
    internal class MultiBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Cast<bool>().All(x => x);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}