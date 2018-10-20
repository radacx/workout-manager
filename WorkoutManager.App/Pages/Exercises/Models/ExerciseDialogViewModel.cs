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

        public ICommand AddPrimaryMuscle { get; }
        
        public ICommand RemovePrimaryMuscle { get; }
        
        public ICommand AddSecondaryMuscle { get; }
        
        public ICommand RemoveSecondaryMuscle { get; }
        
        public ObservedCollection<ExercisedMuscle> PrimaryExercisedMuscles { get; set; }
        
        public ObservedCollection<ExercisedMuscle> SecondaryExercisedMuscles { get; set; }

        private bool IsMuscleAvailable(Muscle muscle)
            => !PrimaryExercisedMuscles.Any(exercisedMuscle => exercisedMuscle.Muscle.Equals(muscle))
                && !SecondaryExercisedMuscles.Any(exercisedMuscle => exercisedMuscle.Muscle.Equals(muscle));
        
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
                    PrimaryExercisedMuscles = new ObservedCollection<ExercisedMuscle>(
                        Exercise.PrimaryMuscles,
                        muscle =>
                        {
                            Exercise.AddPrimaryMuscle(muscle);
                            availableMusclesShape.Apply();
                        },
                        muscle =>
                        {
                            Exercise.RemovePrimaryMuscle(muscle);
                            availableMusclesShape.Apply();
                        }
                    );
                    
                    SecondaryExercisedMuscles = new ObservedCollection<ExercisedMuscle>(
                        Exercise.SecondaryMuscles,
                        muscle =>
                        {
                            Exercise.AddSecondaryMuscle(muscle);
                            availableMusclesShape.Apply();
                        },
                        muscle =>
                        {
                            Exercise.RemoveSecondaryMuscle(muscle);
                            availableMusclesShape.Apply();
                        }
                    );
                }
            };
            
            AddPrimaryMuscle = new Command<Muscle>(muscle => PrimaryExercisedMuscles.Add(new ExercisedMuscle(muscle)));

            RemovePrimaryMuscle = new Command<ExercisedMuscle>(muscle => PrimaryExercisedMuscles.Remove(muscle));
            
            AddSecondaryMuscle = new Command<Muscle>(muscle => SecondaryExercisedMuscles.Add(new ExercisedMuscle(muscle)));
            
            RemoveSecondaryMuscle = new Command<ExercisedMuscle>(muscle => SecondaryExercisedMuscles.Remove(muscle));
            
            Task.Run(
                () =>
                {
                    AvailableMuscles.AddRange(muscleRepository.GetAll());
                    
                    OnPropertyChanged(string.Empty);
                });
        }
    }
}