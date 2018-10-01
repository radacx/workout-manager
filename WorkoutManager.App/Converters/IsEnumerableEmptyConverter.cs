using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace WorkoutManager.App.Converters
{
    internal class IsCollectionEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICollection collection)
            {
                return collection.Count == 0;
            }
            
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}