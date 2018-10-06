using System;
using System.Globalization;
using System.Windows.Data;
using WorkoutManager.App.Pages.Exercises.Models;
using WorkoutManager.App.Pages.TrainingLog.Models;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.ExerciseSet;

namespace WorkoutManager.App.Converters
{
    internal class AddExerciseSetConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
            {
                return null;
            }

            if (!(values[0] is ContractionType type) || !(values[1] is ObservedCollection<IExerciseSet> sets))
            {
                return null;
            }

            return new AddExerciseSetParameters
            {
                Sets = sets,
                Type = type
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}