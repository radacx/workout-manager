using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace WorkoutManager.App
{   
    public class GetMultiSelectItemText : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
            {
                return null;
            }

            var converter = values.OfType<Func<object, string>>().FirstOrDefault();
            var selection = values.OfType<MultiSelectItem>()?.FirstOrDefault();

            if (selection == null)
            {
                return null;
            }

            var item = selection.Item;
            var itemType = item.GetType();
  
            if (converter == null && itemType != typeof(string))
            {
                return null;
            }
            
            return converter == null ? item : converter(item);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}