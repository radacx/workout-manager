using System.Linq;
using WorkoutManager.App.Structures.Collections.Common;
using WorkoutManager.App.Structures.ViewModels;
using WorkoutManager.App.Validation.Validators;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.Pages.Exercises.Dialogs.ExerciseDialog
{
    internal class ExerciseViewModel : ViewModelBase
    {
        private readonly int _id;
        
        private string _name;
        private string _relativeBodyweight;

        private double _relativeBodyweightValue;
        
        public string Name
        {
            get => _name;
            set
            {
                var validator = new TextLengthValidator(minimum: 1);
                var validationResults = validator.Validate(value);
                SetValidationResults(validationResults);

                if (!SetField(ref _name, value) || validationResults != null) { }
            }
        }

        public string RelativeBodyweight
        {
            get => _relativeBodyweight;
            set
            {
                var validator = new DoubleValidator(minimum: 0, maximum: 100);
                var validationResults = validator.Validate(value);
                SetValidationResults(validationResults);
                
                if (!SetField(ref _relativeBodyweight, value) || validationResults != null)
                {
                    return;
                }

                _relativeBodyweightValue = double.Parse(_relativeBodyweight);
            }
        }

        public ContractionType ContractionType { get; set; }
        
        public ObservedCollection<ExercisedMuscle> Muscles { get; set; }
        
        private ExerciseViewModel(int id)
        {
            _id = id;
        }

        public static ExerciseViewModel FromModel(Exercise model) => new ExerciseViewModel(model.Id)
        {
            Name = model.Name,
            ContractionType = model.ContractionType,
            RelativeBodyweight = $"{model.RelativeBodyweight}",
            Muscles = new ObservedCollection<ExercisedMuscle>(
                model.Muscles,
                model.AddMuscle,
                model.RemoveMuscle
            ),
        };

        public Exercise ToModel() => new Exercise
        {
            Id = _id,
            Name = Name,
            RelativeBodyweight = _relativeBodyweightValue,
            ContractionType = ContractionType,
            Muscles = Muscles.ToArray(),
        };
    }
}