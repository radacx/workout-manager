using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public List<Muscle> Muscles { get; } = new List<Muscle>();

        public IEnumerable<Muscle> SelectedPrimaryMuscles { get; set; }

        public IEnumerable<Muscle> SelectedSecondaryMuscles { get; set; }
        
        public ExerciseDialogViewModel(Repository<Muscle> muscleRepository)
        {
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName != nameof(Exercise))
                {
                    return;
                }

                SelectedPrimaryMuscles = new ObservedCollection<Muscle>(
                    Exercise.PrimaryMuscles,
                    Exercise.AddPrimaryMuscle,
                    Exercise.RemovePrimaryMuscle
                );
                
                SelectedSecondaryMuscles = new ObservedCollection<Muscle>(
                    Exercise.SecondaryMuscles,
                    Exercise.AddSecondaryMuscle,
                    Exercise.RemoveSecondaryMuscle
                ); 
            };
            
            Task.Run(
                () =>
                {
                    Muscles.AddRange(muscleRepository.GetAll().OrderBy(muscle => muscle.Name));
                    OnPropertyChanged(string.Empty);
                });
        }
    }
}