using System;
using System.Windows.Input;
using Force.DeepCloner;
using WorkoutManager.App.Pages.Exercises.Models;
using WorkoutManager.App.Pages.TrainingLog.Dialogs;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.ExerciseSet;
using WorkoutManager.Contract.Models.Sessions;

namespace WorkoutManager.App.Pages.TrainingLog.Models
{
    internal class SessionExerciseViewModel 
    {        
        public SessionExercise Exercise { get; set; }
        
        public ObservedCollection<IExerciseSet> Sets { get; }

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
        
        public SessionExerciseViewModel(SessionExercise exercise)
        {
            Exercise = exercise;

            Sets = new ObservedCollection<IExerciseSet>(exercise.Sets, exercise.AddSet, exercise.RemoveSet);
            
            OpenAddExerciseSetDialog = new Command<ContractionType>(
                type =>
                {
                    var set = CreateSet(type);

                    var viewModel = new ExerciseSetDialogViewModel(set)
                    {
                        SaveButtonTitle = "Create"
                    };

                    var dialogResult = DialogBuilder.Create<ExerciseSetDialog>().WithContext(viewModel).Show();
                    
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

                    var viewModel = new ExerciseSetDialogViewModel(setClone)
                    {
                        SaveButtonTitle = "Save"
                    };

                    var dialogResult = DialogBuilder.Create<ExerciseSetDialog>().WithContext(viewModel).Show();
                    
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