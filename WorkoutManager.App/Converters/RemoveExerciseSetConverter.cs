using System;
using System.Globalization;
using System.Windows.Data;
using WorkoutManager.App.TrainingLog.Models;
using WorkoutManager.Contract.Models.ExerciseSet;
using WorkoutManager.Contract.Models.Sessions;

namespace WorkoutManager.App.Converters
{
    public class RemoveExerciseSetConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2)
            {
                return null;
            }

            if (!(values[0] is SessionExercise exercise) || !(values[1] is IExerciseSet set))
            {
                return null;
            }
            
            var parameters = new RemoveExerciseSetParameters
            {
                Exercise = exercise,
                Set = set
            };

            return parameters;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}