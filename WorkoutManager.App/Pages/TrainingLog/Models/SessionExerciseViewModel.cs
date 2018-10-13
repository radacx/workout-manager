using System.Linq;
using System.Windows.Input;
using Force.DeepCloner;
using PubSub.Core;
using WorkoutManager.App.Events;
using WorkoutManager.App.Pages.TrainingLog.Dialogs;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Models.ExerciseSet;
using WorkoutManager.Contract.Models.Sessions;

namespace WorkoutManager.App.Pages.TrainingLog.Models
{
    internal class SessionExerciseViewModel : ViewModelBase
    {
        private SessionExercise _exercise;
        public SessionExercise Exercise
        {
            get => _exercise;
            set => SetField(ref _exercise, value);
        }

        public ObservedCollection<IExerciseSet> Sets { get; set; }

        public ICommand OpenAddExerciseSetDialog { get; }
        
        public ICommand OpenEditExerciseSetDialog { get; }
        
        public ICommand DeleteSet { get; }
        
        public SessionExerciseViewModel(DialogFactory<ExerciseSetDialog, ExerciseSetDialogViewModel> exerciseSetDialogFactory, Hub eventAggregator)
        {
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Exercise))
                {
                    Sets = new ObservedCollection<IExerciseSet>(
                        Exercise.Sets,
                        Exercise.AddSet,
                        Exercise.RemoveSet
                    );
                }
            };

            void OpenAddExerciseSet(SessionExercise sessionExercise)
            {
                var set = sessionExercise.Sets.LastOrDefault()?.DeepClone()
                    ?? SetCreator.Create(sessionExercise.Exercise.ContractionType);

                var dialog = exerciseSetDialogFactory.Get();
                dialog.Data.ExerciseSet = set;
                dialog.Data.SaveButtonTitle = "Create";

                var dialogResult = dialog.Show();

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
            
            OpenEditExerciseSetDialog = new Command<IExerciseSet>(
                set =>
                {
                    var setClone = set.DeepClone();

                    var dialog = exerciseSetDialogFactory.Get();
                    dialog.Data.ExerciseSet = setClone;
                    dialog.Data.SaveButtonTitle = "Save";

                    var dialogResult = dialog.Show();
                    
                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    Sets.Replace(set, setClone);
                });
            
            DeleteSet = new Command<IExerciseSet>(set => Sets.Remove(set));
        }
    }
}