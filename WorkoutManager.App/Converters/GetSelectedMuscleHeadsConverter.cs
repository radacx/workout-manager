using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.Converters
{
    internal class GetSelectedMuscleHeadsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
            {
                return null;
            }

            if (!(values[0] is MuscleGroup muscleGroup)
                || !(values[1] is Dictionary<MuscleGroup, IEnumerable<MuscleHead>> selectedMusclesHeads))
            {
                return null;
            }

            return selectedMusclesHeads[muscleGroup];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}