using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Pages.Exercises.Models
{
    internal class ExerciseDialogViewModel : INotifyPropertyChanged
    {
        public bool IsBodyweightExercise { get; set; }
        
        public string SaveButtonTitle { get; set; }
        
        public Exercise Exercise { get; }

        public List<MuscleGroup> MuscleGroups { get; } = new List<MuscleGroup>();

        public List<JointMotion> Motions { get; } = new List<JointMotion>();
        
        public Func<object, string> GetJointMotionText { get; }
        
        public Func<object, string> GetMuscleGroupText { get; }
        
        public Func<object, string> GetMuscleHeadText { get; }
        
        public IEnumerable<MuscleGroup> SelectedPrimaryMuscleGroups { get; }
        
        public IEnumerable<MuscleGroup> SelectedSecondaryMuscleGroups { get; }

        public Dictionary<MuscleGroup, IEnumerable<MuscleHead>> SelectedPrimaryMusclesHeads { get; }
        
        public Dictionary<MuscleGroup, IEnumerable<MuscleHead>> SelectedSecondaryMusclesHeads { get; }
        
        private static MuscleGroup Convert(ExercisedMuscle muscle) => muscle.MuscleGroup;
        
        public ExerciseDialogViewModel(Exercise exercise, Repository<JointMotion> motionsRepository, Repository<MuscleGroup> muscleGroupRepository)
        {
            Exercise = exercise;

            var selectedPrimaryMuscleGroups = new ObservableCollection<MuscleGroup>(exercise.PrimaryMuscles.Select(Convert));
            SelectedPrimaryMuscleGroups = selectedPrimaryMuscleGroups;
            
            var selectedSecondaryMuscleGroups = new ObservableCollection<MuscleGroup>(exercise.SecondaryMuscles.Select(Convert));
            SelectedSecondaryMuscleGroups = selectedSecondaryMuscleGroups;

            SelectedPrimaryMusclesHeads = new Dictionary<MuscleGroup, IEnumerable<MuscleHead>>();
            
            foreach (var muscle in exercise.PrimaryMuscles)
            {
               SelectedPrimaryMusclesHeads.Add(muscle.MuscleGroup, muscle.UsedHeads);
            }
            
            SelectedSecondaryMusclesHeads = new Dictionary<MuscleGroup, IEnumerable<MuscleHead>>();
            
            foreach (var muscle in exercise.SecondaryMuscles)
            {
                SelectedSecondaryMusclesHeads.Add(muscle.MuscleGroup, muscle.UsedHeads);
            }

            selectedPrimaryMuscleGroups.CollectionChanged += (sender, args) =>
            {
                if (args.NewItems != null)
                {
                    foreach (MuscleGroup muscleGroup in args.NewItems)
                    {
                        var muscle = new ExercisedMuscle(muscleGroup);
                        exercise.AddPrimaryMuscle(muscle);
                        SelectedPrimaryMusclesHeads.Add(muscleGroup, muscle.UsedHeads);
                    }
                }

                if (args.OldItems != null)
                {
                    foreach (MuscleGroup muscleGroup in args.OldItems)
                    {
                        exercise.RemovePrimaryMuscle(muscleGroup);
                        SelectedPrimaryMusclesHeads.Remove(muscleGroup);
                    }
                }  
            };
            
            selectedSecondaryMuscleGroups.CollectionChanged += (sender, args) =>
            {
                if (args.NewItems != null)
                {
                    foreach (MuscleGroup muscleGroup in args.NewItems)
                    {
                        var muscle = new ExercisedMuscle(muscleGroup);
                        exercise.AddSecondaryMuscle(muscle);
                        SelectedSecondaryMusclesHeads.Add(muscleGroup, muscle.UsedHeads);
                    }
                }

                if (args.OldItems != null)
                {
                    foreach (MuscleGroup muscleGroup in args.OldItems)
                    {
                        exercise.RemoveSecondaryMuscle(muscleGroup);
                        SelectedSecondaryMusclesHeads.Remove(muscleGroup);
                    }
                }  
            };
            
            Task.Run(
                () =>
                {
                    Motions.AddRange(motionsRepository.GetAll());
                    MuscleGroups.AddRange(muscleGroupRepository.GetAll());
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
                });
            
            GetJointMotionText = motion => (motion as JointMotion)?.Name;

            GetMuscleGroupText = muscleGroup => (muscleGroup as MuscleGroup)?.Name;

            GetMuscleHeadText = muscleHead => (muscleHead as MuscleHead)?.Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}