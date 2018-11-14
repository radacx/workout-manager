using System;
using WorkoutManager.App.Pages.TrainingLog.Dialogs.ExerciseSetDialog.Dynamic;
using WorkoutManager.App.Pages.TrainingLog.Dialogs.ExerciseSetDialog.Isometric;
using WorkoutManager.App.Structures.ViewModels;
using WorkoutManager.App.Validation.Validators;
using WorkoutManager.Contract.Models.Sessions.Sets;
using DynamicExerciseSet = WorkoutManager.Contract.Models.Sessions.Sets.DynamicExerciseSet;

namespace WorkoutManager.App.Pages.TrainingLog.Dialogs.ExerciseSetDialog
{
    internal abstract class ExerciseSetViewModel : ViewModelBase
    {
        private string _weight;

        protected double WeightValue;
        
        public string Weight
        {
            get => _weight;
            set
            {
                var validator = new DoubleValidator(minimum: 0);
                var validationResults = validator.Validate(value);
                SetValidationResults(validationResults);

                if (!SetField(ref _weight, value) || validationResults != null)
                {
                    return;
                }

                WeightValue = double.Parse(value);
            }
        }

        public abstract ExerciseSet ToModel();

        public static ExerciseSetViewModel FromModel(ExerciseSet model)
        {
            switch (model)
            {
                case DynamicExerciseSet dynamicSet:

                    return DynamicExerciseSetViewModel.FromModel(dynamicSet);
                case IsometricExerciseSet isometricSet:

                    return IsometricExerciseSetViewModel.FromModel(isometricSet);
                default:
                    throw new ArgumentException($"Unknown exercise set type: {model?.GetType()}");
            }
        }
    }
}