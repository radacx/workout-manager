using System;
using System.Globalization;
using System.Windows.Data;
using WorkoutManager.App.Pages.TrainingLog.Models;
using WorkoutManager.Contract.Models.Sessions;

namespace WorkoutManager.App.Converters
{
    internal class GetSessionExerciseViewModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SessionExercise exercise)
            {
                return new SessionExerciseViewModel(exercise);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}