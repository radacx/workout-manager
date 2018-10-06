using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Service;

namespace WorkoutManager.App.Converters
{
    internal class GetExerciseProgressConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
            {
                return null;
            }

            if (!(values[0] is IEnumerable<TrainingSession> sessions) || !(values[1] is Exercise exercise))
            {
                return null;
            }

            return new ProgressService().GetTotalVolume(sessions, new ProgressFilter()
            {
                Exercise = exercise
            });
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}