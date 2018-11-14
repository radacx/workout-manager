using WorkoutManager.App.Structures.ViewModels;
using WorkoutManager.App.Validation.Validators;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.Pages.Exercises.Dialogs.Models
{
    internal class ExercisedMuscleViewModel : ViewModelBase
    {
        private string _relativeEngagement;

        private double _relativeEngagementValue;
        private Muscle _muscle;

        public string RelativeEngagement
        {
            get => _relativeEngagement;
            set
            {
                var validator = new DoubleValidator(minimum: 0.05, maximum: 100);
                var validationResults = validator.Validate(value);
                SetValidationResults(validationResults);

                if (!SetField(ref _relativeEngagement, value) || validationResults != null)
                {
                    return;
                }

                _relativeEngagementValue = double.Parse(value);
            }
        }

        public Muscle Muscle
        {
            get => _muscle;
            set
            {
                var validationResults = value == null ? new[] { "Select a muscle." } : null;
                SetValidationResults(validationResults);

                if (!SetField(ref _muscle, value) || validationResults != null) { }
            }
        }

        public static ExercisedMuscleViewModel FromModel(ExercisedMuscle model)
            => new ExercisedMuscleViewModel
            {
                Muscle = model.Muscle,
                RelativeEngagement = $"{model.RelativeEngagement}",
            };
        
        public ExercisedMuscle ToModel() => new ExercisedMuscle
        {
            RelativeEngagement = _relativeEngagementValue,
            Muscle = Muscle,
        };
    }
}