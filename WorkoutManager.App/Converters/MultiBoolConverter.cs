using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace WorkoutManager.App.Converters
{
    internal enum MultiBoolConverterParam
    {
        And,
        Or,
    }
    
    internal class MultiBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Any(value => value == DependencyProperty.UnsetValue))
            {
                return false;
            }
            
            switch (parameter) {
                case MultiBoolConverterParam param:

                    switch (param)
                    {
                        case MultiBoolConverterParam.And:

                            return values.Cast<bool>().All(x => x);
                        case MultiBoolConverterParam.Or:

                            return values.Cast<bool>().Any(x => x);
                        default:
                            throw new ArgumentException("Invalid parameter");
                    }

                case null:

                    return values.Cast<bool>().All(x => x);
                default:

                    return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}