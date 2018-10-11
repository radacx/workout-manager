using System;
using System.Windows.Input;
using Force.DeepCloner;
using WorkoutManager.App.Pages.TrainingLog.Dialogs;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Models.Exercises;
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
        
        private static IExerciseSet CreateSet(ContractionType type)
        {
            switch (type)
            {
                case ContractionType.Dynamic:
                    return new DynamicExerciseSet();
                
                case ContractionType.Isometric:
                    return new IsometricExerciseSet();
                
                default:
                    throw new ArgumentException($"Invalid contraction type: {type}");
            }
        }
        
        public SessionExerciseViewModel(DialogFactory<ExerciseSetDialog, ExerciseSetDialogViewModel> exerciseSetDialogFactory)
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
                
            OpenAddExerciseSetDialog = new Command<ContractionType>(
                type =>
                {
                    var set = CreateSet(type);

                    var dialog = exerciseSetDialogFactory.Get();
                    dialog.Data.ExerciseSet = set;
                    dialog.Data.SaveButtonTitle = "Create";

                    var dialogResult = dialog.Show();
                    
                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    Sets.Add(set);
                });
            
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