using System;
using System.Globalization;
using System.Windows.Data;
using WorkoutManager.App.Pages.TrainingLog.Models;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Models.Sessions;

namespace WorkoutManager.App.Pages.TrainingLog.Converters
{
    internal class GetSessionExerciseViewModelConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
            {
                return null;
            }

            if (!(values[0] is SessionExercise exercise) || !(values[1] is ViewModelFactory<SessionExerciseViewModel> exerciseViewModelFactory))
            {
                return null;
            }

            var viewModel = exerciseViewModelFactory.Get();
            viewModel.Exercise = exercise;

            return viewModel;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}