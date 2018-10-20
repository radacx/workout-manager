using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutManager.App.Structures;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Pages.Exercises.Models
{
    internal class ExerciseDialogViewModel : ViewModelBase
    {
        private Exercise _exercise;
        public bool IsBodyweightExercise { get; set; }
        
        public string SaveButtonTitle { get; set; }

        public Exercise Exercise
        {
            get => _exercise;
            set => SetField(ref _exercise, value);
        }

        public BulkObservableCollection<Muscle> AvailableMuscles { get; } = new BulkObservableCollection<Muscle>();

        public ICommand AddMuscle { get; }
        
        public ICommand RemoveMuscle { get; }
        
        public ObservedCollection<ExercisedMuscle> ExercisedMuscles { get; set; }

        private bool IsMuscleAvailable(Muscle muscle)
            => !ExercisedMuscles.Any(exercisedMuscle => exercisedMuscle.Muscle.Equals(muscle));
        
        public ExerciseDialogViewModel(Repository<Muscle> muscleRepository)
        {
            var availableMusclesShape = AvailableMuscles.ShapeView()
                .Where(IsMuscleAvailable)
                .OrderBy(muscle => muscle.Name);
            
            availableMusclesShape.Apply();

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Exercise))
                {
                    ExercisedMuscles = new ObservedCollection<ExercisedMuscle>(
                        Exercise.Muscles,
                        muscle =>
                        {
                            Exercise.AddMuscle(muscle);
                            availableMusclesShape.Apply();
                        },
                        muscle =>
                        {
                            Exercise.RemoveMuscle(muscle);
                            availableMusclesShape.Apply();
                        }
                    );
                }
            };
            
            AddMuscle = new Command<Muscle>(muscle => ExercisedMuscles.Add(new ExercisedMuscle(muscle, 100d)));

            RemoveMuscle = new Command<ExercisedMuscle>(muscle => ExercisedMuscles.Remove(muscle));
            
            Task.Run(
                () =>
                {
                    AvailableMuscles.AddRange(muscleRepository.GetAll());
                    
                    OnPropertyChanged(string.Empty);
                });
        }
    }
}