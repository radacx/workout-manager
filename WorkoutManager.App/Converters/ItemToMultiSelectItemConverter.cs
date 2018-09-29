using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using WorkoutManager.App.Controls.MultiSelect;

namespace WorkoutManager.App.Converters
{
    internal class ItemToMultiSelectItemConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
            {
                return null;
            }

            if (!(values[0] is IEnumerable<object> items) || !(values[1] is IEnumerable<object> selectedItems))
            {
                return null;
            }

            return items.Select(item => new MultiSelectItem(item, selectedItems.Contains(item)));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}