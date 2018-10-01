using System;
using System.Globalization;
using System.Windows.Data;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.ExerciseSet;
using WorkoutManager.Contract.Models.User;

namespace WorkoutManager.App.Converters
{
    internal class ExerciseSetToTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
            {
                return null;
            }
            
            if (!(values[0] is IExerciseSet exerciseSet) || !(values[1] is UserPreferences userPreferences))
            {
                return null;
            }

            var weightUnits = userPreferences.WeightUnits.GetDescription();
            
            switch (exerciseSet)
            {
                case DynamicExerciseSet dynamicSet:

                    return $"{dynamicSet.Reps} @ {dynamicSet.Weight} {weightUnits}";
                case IsometricExerciseSet isometricSet:

                    return $"{isometricSet.Duration} @ {isometricSet.Weight} {weightUnits}";
                
                default:

                    throw new ArgumentException($"Unknown exercise set: {exerciseSet.GetType().FullName}");
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}