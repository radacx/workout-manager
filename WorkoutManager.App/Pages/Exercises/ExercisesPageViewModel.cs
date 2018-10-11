using System.Threading.Tasks;
using System.Windows.Input;
using Force.DeepCloner;
using WorkoutManager.App.Pages.Exercises.Dialogs;
using WorkoutManager.App.Pages.Exercises.Models;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Service.Services;

namespace WorkoutManager.App.Pages.Exercises
{
    internal class ExercisesPageViewModel
    {
        public BulkObservableCollection<Exercise> Exercises { get; } = new BulkObservableCollection<Exercise>();
        
        public ICommand OpenCreateExerciseModalDialog { get; }
        
        public ICommand OpenEditExerciseModalDialog { get; }

        public ICommand Delete { get; }
        
        private void LoadExercises() => Exercises.AddRange(_exerciseService.GetAll());

        private readonly ExerciseService _exerciseService;
        
        public ExercisesPageViewModel(ExerciseService exerciseService, DialogFactory<ExerciseDialog, ExerciseDialogViewModel> exerciseDialogFactory)
        {
            _exerciseService = exerciseService;
            
            OpenCreateExerciseModalDialog = new Command(
                () =>
                {
                    var exercise = new Exercise();

                    var dialog = exerciseDialogFactory.Get();
                    dialog.Data.Exercise = exercise;
                    dialog.Data.SaveButtonTitle = "Create";
                    
                    var dialogResult = dialog.Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    if (!dialog.Data.IsBodyweightExercise)
                    {
                        exercise.RelativeBodyweight = 0;
                    }
                    
                    Exercises.Add(exercise);

                    Task.Run(() => _exerciseService.Create(exercise));
                }
            );

            Delete = new Command<Exercise>(
                exercise =>
                {
                    Exercises.Remove(exercise);

                    Task.Run(() => _exerciseService.Delete(exercise));
                });

            OpenEditExerciseModalDialog = new Command<Exercise>(
                exercise =>
                {
                    var exerciseClone = exercise.DeepClone();

                    var dialog = exerciseDialogFactory.Get();
                    dialog.Data.Exercise = exerciseClone;
                    dialog.Data.SaveButtonTitle = "Save";
                    dialog.Data.IsBodyweightExercise = exercise.RelativeBodyweight > 0;
                    
                    var dialogResult = dialog.Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    if (!dialog.Data.IsBodyweightExercise)
                    {
                        exerciseClone.RelativeBodyweight = 0;
                    }
                    Exercises.Replace(exercise, exerciseClone);
                    
                    Task.Run(() => _exerciseService.Update(exerciseClone));
                });

            Task.Run(() => LoadExercises());
        }
    }
}
