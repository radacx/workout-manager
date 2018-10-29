using System;
using System.Globalization;
using System.Windows.Data;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Exercises.Sets;
using WorkoutManager.Contract.Models.User;

namespace WorkoutManager.App.Pages.TrainingLog.Converters
{
    internal class ExerciseSetToTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
            {
                return null;
            }
            
            if (!(values[0] is ExerciseSet exerciseSet) || !(values[1] is UserPreferences userPreferences))
            {
                return null;
            }

            var weightUnits = userPreferences.WeightUnits.GetDescription();

            return $"{exerciseSet} {weightUnits}";

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}