using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using WorkoutManager.App.Structures;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Pages.Exercises.Models
{
    internal class ExerciseDialogViewModel : ViewModelBase, INotifyPropertyChanged
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

        public List<JointMotion> Motions { get; } = new List<JointMotion>();
        
        public Func<object, string> GetJointMotionText { get; }
        
        public Func<object, string> GetMuscleText { get; }

        public IEnumerable<JointMotion> SelectedMotions { get; set; }

        public IEnumerable<Muscle> SelectedPrimaryMuscles { get; set; }

        public IEnumerable<Muscle> SelectedSecondaryMuscles { get; set; }
        
        public ExerciseDialogViewModel(Repository<JointMotion> motionsRepository, Repository<Muscle> muscleRepository)
        {
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName != nameof(Exercise))
                {
                    return;
                }
                
                SelectedMotions = new ObservedCollection<JointMotion>(
                    Exercise.Motions,
                    Exercise.AddMotion,
                    Exercise.RemoveMotion
                );
                
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
                    Motions.AddRange(motionsRepository.GetAll().OrderBy(motion => motion.Name));
                    Muscles.AddRange(muscleRepository.GetAll().OrderBy(muscle => muscle.Name));
                    OnPropertyChanged(string.Empty);
                });
            
            GetJointMotionText = motion => (motion as JointMotion)?.Name;

            GetMuscleText = muscle => (muscle as Muscle)?.Name;
        }
    }
}