using System;
using System.Globalization;
using System.Windows.Data;
using WorkoutManager.App.Controls.MultiSelect;

namespace WorkoutManager.App.Converters
{   
    internal class GetMultiSelectItemTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
            {
                return null;
            }

            if (!(values[0] is MultiSelectItem selection))
            {
                return null;
            }
            
            if (!(values[1] is Func<object, string> converter))
            {
                return selection.Item.ToString();
            }

            return converter(selection.Item);

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}