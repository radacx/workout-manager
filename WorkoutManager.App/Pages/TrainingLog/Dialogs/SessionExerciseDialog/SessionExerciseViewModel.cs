using System.Linq;
using WorkoutManager.App.Structures.Collections.Common;
using WorkoutManager.App.Structures.ViewModels;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Contract.Models.Sessions.Sets;

namespace WorkoutManager.App.Pages.TrainingLog.Dialogs.SessionExerciseDialog
{
    internal class SessionExerciseViewModel : ViewModelBase
    {
        private Exercise _exercise;      
        
        public Exercise Exercise
        {
            get => _exercise;
            set
            {
                var validationResults = value == null ? new[] { "Select an exercise." } : null;
                SetValidationResults(validationResults);

                if (!SetField(ref _exercise, value) || validationResults != null) { }
            }
        }

        public ObservedCollection<ExerciseSet> Sets { get; set; }
        
        public static SessionExerciseViewModel FromModel(SessionExercise model)
        {
            var viewModel = new SessionExerciseViewModel
            {
                Exercise = model.Exercise,
                Sets = new ObservedCollection<ExerciseSet>(model.Sets, model.AddSet, model.RemoveSet),
            };

            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Exercise))
                {
                    viewModel.Sets.Clear();
                }
            };
            
            return viewModel;
        }

        public SessionExercise ToModel() => new SessionExercise
        {
            Exercise = Exercise,
            Sets = Sets.ToArray(),
        };
    }
}