using WorkoutManager.App.Structures.ViewModels;
using WorkoutManager.App.Validation.Validators;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.Pages.Muscles.Dialogs.MuscleDialog
{
    internal class MuscleViewModel : ViewModelBase
    {
        private readonly int _id;

        private string _name;

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

        private MuscleViewModel(int id)
        {
            _id = id;
        }

        public static MuscleViewModel FromModel(Muscle muscle) => new MuscleViewModel(muscle.Id)
        {
            Name = muscle.Name,
        };

        public Muscle ToModel() => new Muscle
        {
            Name = Name,
            Id = _id,
        };
    }
}