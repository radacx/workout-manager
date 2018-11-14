using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace WorkoutManager.App.Converters
{
    internal class EnumerableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is IEnumerable enumerable))
            {
                return null;
            }

            return string.Join(", ", enumerable.Cast<object>().Select(x => x.ToString()));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}