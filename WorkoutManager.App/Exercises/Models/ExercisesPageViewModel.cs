using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutManager.App.Exercises.UserControls;
using WorkoutManager.App.Misc;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;
using WorkoutManager.Service;

namespace WorkoutManager.App.Exercises.Models
{
    internal class ExercisesPageViewModel
    {
        public BulkObservableCollection<Exercise> Exercises { get; } = new BulkObservableCollection<Exercise>();
        
        public ICommand OpenCreateExerciseModalDialog { get; }
        
        public ICommand OpenEditExerciseModalDialog { get; }

        public ICommand Delete { get; }
        
        private void LoadExercises() => Exercises.AddRange(_exerciseService.GetAll());

        private readonly ExerciseService _exerciseService;
        
        public ExercisesPageViewModel(ExerciseService exerciseService, Repository<JointMotion> motionsRepository, Repository<MuscleGroup> muscleGroupRepository)
        {
            _exerciseService = exerciseService;
            
            OpenCreateExerciseModalDialog = new Command(
                () =>
                {
                    var exercise = new Exercise();

                    var dialog = new ExerciseDialog
                    {
                        DataContext = new ExerciseDialogViewModel(exercise, motionsRepository, muscleGroupRepository)
                        {
                            SaveButtonTitle = "Create"
                        }
                    };

                    var dialogResult = dialog.ShowDialog();

                    if (!dialogResult.HasValue || !dialogResult.Value)
                    {
                        return;
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

                    var dialog = new ExerciseDialog
                    {
                        DataContext = new ExerciseDialogViewModel(exerciseClone, motionsRepository, muscleGroupRepository)
                        {
                            SaveButtonTitle = "Save"
                        }
                    };

                    var dialogResult = dialog.ShowDialog();

                    if (!dialogResult.HasValue || !dialogResult.Value)
                    {
                        return;
                    }

                    Exercises.Replace(oldExercise => oldExercise.Equals(exerciseClone), exerciseClone);
                    
                    Task.Run(() => _exerciseService.Update(exerciseClone));
                });

            Task.Run(() => LoadExercises());
        }
    }
}
