using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutManager.App.Pages.Exercises.Dialogs;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;
using WorkoutManager.Service;

namespace WorkoutManager.App.Pages.Exercises.Models
{
    internal class ExercisesPageViewModel
    {
        public BulkObservableCollection<Exercise> Exercises { get; } = new BulkObservableCollection<Exercise>();
        
        public ICommand OpenCreateExerciseModalDialog { get; }
        
        public ICommand OpenEditExerciseModalDialog { get; }

        public ICommand Delete { get; }
        
        private void LoadExercises() => Exercises.AddRange(_exerciseService.GetAll());

        private readonly ExerciseService _exerciseService;
        private readonly DialogViewer<ExerciseDialog> _exerciseDialogViewer;
        
        public ExercisesPageViewModel(ExerciseService exerciseService, Repository<JointMotion> motionsRepository, Repository<MuscleGroup> muscleGroupRepository, DialogViewer<ExerciseDialog> exerciseDialogViewer)
        {
            _exerciseService = exerciseService;
            _exerciseDialogViewer = exerciseDialogViewer;
            
            OpenCreateExerciseModalDialog = new Command(
                () =>
                {
                    var exercise = new Exercise();

                    var viewModel = new ExerciseDialogViewModel(exercise, motionsRepository, muscleGroupRepository)
                    {
                        SaveButtonTitle = "Create"
                    };

                    var dialogResult = _exerciseDialogViewer.WithContext(viewModel).Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    if (!viewModel.IsBodyweightExercise)
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
                    var exerciseClone = exercise.Clone();

                    var viewModel = new ExerciseDialogViewModel(exerciseClone, motionsRepository, muscleGroupRepository)
                    {
                        SaveButtonTitle = "Save",
                        IsBodyweightExercise = exercise.RelativeBodyweight > 0
                    };

                    var dialogResult = _exerciseDialogViewer.WithContext(viewModel).Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    if (!viewModel.IsBodyweightExercise)
                    {
                        exerciseClone.RelativeBodyweight = 0;
                    }
                    Exercises.Replace(oldExercise => oldExercise.Equals(exerciseClone), exerciseClone);
                    
                    Task.Run(() => _exerciseService.Update(exerciseClone));
                });

            Task.Run(() => LoadExercises());
        }
    }
}
