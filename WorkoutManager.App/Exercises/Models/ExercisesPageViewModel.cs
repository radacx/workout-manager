using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutManager.App.Exercises.UserControls;
using WorkoutManager.App.Misc;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Exercises.Models
{
    internal class ExercisesPageViewModel
    {
        public BulkObservableCollection<Exercise> Exercises { get; } = new BulkObservableCollection<Exercise>();
        
        public ICommand OpenCreateExerciseModalDialog { get; }
        
        public ICommand OpenEditExerciseModalDialog { get; }

        public ICommand Delete { get; }
        
        private void LoadExercises() => Exercises.AddRange(_exerciseRepository.GetAll());

        private void CreateExercise(Exercise exercise)
        {
            _exercisedMuscleRepository.CreateRange(exercise.PrimaryMuscles);
            _exercisedMuscleRepository.CreateRange(exercise.SecondaryMuscles);
            _exerciseRepository.Create(exercise);
        }

        private void EditExercise(Exercise exercise)
        {
            var newPrimaryMuscles = exercise.PrimaryMuscles.Where(muscle => muscle.Id == 0);
            var newSecondaryMuscle = exercise.SecondaryMuscles.Where(muscle => muscle.Id == 0);
            
            _exercisedMuscleRepository.CreateRange(newPrimaryMuscles);
            _exercisedMuscleRepository.CreateRange(newSecondaryMuscle);
            
            _exerciseRepository.Update(exercise);
        }

        private void DeleteExercise(Exercise exercise) => _exerciseRepository.Delete(exercise);
        
        private readonly Repository<Exercise> _exerciseRepository;

        private readonly Repository<MuscleGroup> _muscleGroupRepository;

        private readonly Repository<ExercisedMuscle> _exercisedMuscleRepository;
        
        public ExercisesPageViewModel(Repository<Exercise> exerciseRepository, Repository<JointMotion> motionsRepository, Repository<MuscleGroup> muscleGroupRepository, Repository<ExercisedMuscle> exercisedMuscleRepository)
        {
            _exerciseRepository = exerciseRepository;
            _muscleGroupRepository = muscleGroupRepository;
            _exercisedMuscleRepository = exercisedMuscleRepository;
            
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

                    Task.Run(() => CreateExercise(exercise));
                }
            );

            Delete = new Command<Exercise>(
                exercise =>
                {
                    Exercises.Remove(exercise);

                    Task.Run(() => DeleteExercise(exercise));
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
                    Task.Run(() => EditExercise(exerciseClone));
                });

            Task.Run(() => LoadExercises());
        }
    }
}
