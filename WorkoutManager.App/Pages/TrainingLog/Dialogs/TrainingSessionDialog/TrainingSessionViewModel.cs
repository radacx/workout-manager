using System;
using System.Linq;
using WorkoutManager.App.Structures.Collections.Common;
using WorkoutManager.App.Structures.ViewModels;
using WorkoutManager.App.Validation.Validators;
using WorkoutManager.Contract.Models.Sessions;

namespace WorkoutManager.App.Pages.TrainingLog.Dialogs.TrainingSessionDialog
{
    internal class TrainingSessionViewModel : ViewModelBase
    {
        private readonly int _id;
        
        private string _bodyweight;

        private double _bodyweightValue;
        
        public string Bodyweight
        {
            get => _bodyweight;
            set
            {
                var validator = new DoubleValidator(minimum: 0);
                var validationResults = validator.Validate(value);
                SetValidationResults(validationResults);

                if (!SetField(ref _bodyweight, value) || validationResults != null)
                {
                    return;
                }

                _bodyweightValue = double.Parse(value);
            }
        }

        public DateTime Date { get; set; }
        
        public ObservedCollection<SessionExercise> Exercises { get; set; }
        
        private TrainingSessionViewModel(int id)
        {
            _id = id;
        }

        public static TrainingSessionViewModel FromModel(TrainingSession model)
            => new TrainingSessionViewModel(model.Id)
            {
                Exercises = new ObservedCollection<SessionExercise>(
                    model.Exercises,
                    model.AddExercise,
                    model.RemoveExercise
                ),
                Date = model.Date,
                Bodyweight = $"{model.Bodyweight}",
            };
        
        public TrainingSession ToModel() => new TrainingSession
        {
            Id = _id,
            Exercises = Exercises.ToArray(),
            Date = Date,
            Bodyweight = _bodyweightValue,
        };
    }
}