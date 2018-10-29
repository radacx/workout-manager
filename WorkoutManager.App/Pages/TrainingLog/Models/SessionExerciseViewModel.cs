using System.Linq;
using System.Windows.Input;
using Force.DeepCloner;
using PubSub.Core;
using WorkoutManager.App.Events;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils;
using WorkoutManager.App.Utils.Dialogs;
using WorkoutManager.Contract.Models.Exercises.Sets;
using WorkoutManager.Contract.Models.Sessions;

namespace WorkoutManager.App.Pages.TrainingLog.Models
{
    internal class SessionExerciseViewModel : ViewModelBase
    {
        private SessionExercise _exercise;

        public string ExerciseSetDialogIdentifier => "ExerciseSetDialog";
        
        public SessionExercise Exercise
        {
            get => _exercise;
            set => SetField(ref _exercise, value);
        }

        public ObservedCollection<ExerciseSet> Sets { get; set; }

        public ICommand OpenAddExerciseSetDialog { get; }
        
        public ICommand OpenEditExerciseSetDialog { get; }
        
        public ICommand DeleteSet { get; }
        
        public SessionExerciseViewModel(DialogViewer dialogViewer, Hub eventAggregator)
        {
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Exercise))
                {
                    Sets = new ObservedCollection<ExerciseSet>(
                        Exercise.Sets,
                        Exercise.AddSet,
                        Exercise.RemoveSet
                    );
                }
            };

            async void OpenAddExerciseSet(SessionExercise sessionExercise)
            {
                var set = sessionExercise.Sets.LastOrDefault()?.DeepClone()
                    ?? SetCreator.Create(sessionExercise.Exercise.ContractionType);

                var dialog = dialogViewer.For<ExerciseSetDialogViewModel>(ExerciseSetDialogIdentifier);
                dialog.Data.ExerciseSet = set;
                dialog.Data.SubmitButtonTitle = "Create";
                dialog.Data.DialogTitle = "Add set";
                    
                var dialogResult = await dialog.Show();

                if (dialogResult != DialogResult.Ok)
                {
                    return;
                }

                Sets.Add(set);
            }

            eventAggregator.Subscribe<SessionExerciseReAddedEvent>(
                seEvent =>
                {
                    if (ReferenceEquals(Exercise, seEvent.SessionExercise))
                    {
                        OpenAddExerciseSet(seEvent.SessionExercise);
                    }
                }
            );
            
            OpenAddExerciseSetDialog = new Command<SessionExercise>(OpenAddExerciseSet);
            
            OpenEditExerciseSetDialog = new Command<ExerciseSet>(
                async set =>
                {
                    var setClone = set.DeepClone();

                    var dialog = dialogViewer.For<ExerciseSetDialogViewModel>(ExerciseSetDialogIdentifier);
                    dialog.Data.ExerciseSet = setClone;
                    dialog.Data.SubmitButtonTitle = "Save";
                    dialog.Data.DialogTitle = "Modify set";
                    
                    var dialogResult = await dialog.Show();
                    
                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    Sets.Replace(set, setClone);
                });
            
            DeleteSet = new Command<ExerciseSet>(set => Sets.Remove(set));
        }
    }
}