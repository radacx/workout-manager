using System;
using System.Globalization;
using System.Windows.Data;
using WorkoutManager.App.Pages.Exercises.Models;
using WorkoutManager.App.Pages.TrainingLog.Models;
using WorkoutManager.Contract.Models.ExerciseSet;
using WorkoutManager.Contract.Models.Sessions;

namespace WorkoutManager.App.Converters
{
    internal class RemoveExerciseSetConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2)
            {
                return null;
            }

            if (!(values[0] is IExerciseSet set) || !(values[1] is ObservedCollection<IExerciseSet> sets))
            {
                return null;
            }
            
            var parameters = new RemoveExerciseSetParameters
            {
                Sets = sets,
                Set = set
            };

            return parameters;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}