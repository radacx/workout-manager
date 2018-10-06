using System;
using System.Collections.Generic;
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
        
        public IEnumerable<JointMotion> SelectedMotions { get; }
        
        public IEnumerable<MuscleGroup> SelectedPrimaryMuscleGroups { get; }
        
        public IEnumerable<MuscleGroup> SelectedSecondaryMuscleGroups { get; }

        public Dictionary<MuscleGroup, IEnumerable<MuscleHead>> SelectedPrimaryMusclesHeads { get; }
        
        public Dictionary<MuscleGroup, IEnumerable<MuscleHead>> SelectedSecondaryMusclesHeads { get; }
        
        private static MuscleGroup GetMuscleGroup(ExercisedMuscle muscle) => muscle.MuscleGroup;
        
        public ExerciseDialogViewModel(Exercise exercise, Repository<JointMotion> motionsRepository, Repository<MuscleGroup> muscleGroupRepository)
        {
            Exercise = exercise;

            SelectedPrimaryMusclesHeads = new Dictionary<MuscleGroup, IEnumerable<MuscleHead>>();
            
            foreach (var muscle in exercise.PrimaryMuscles)
            {
                var muscleHeads = new ObservedCollection<MuscleHead>(muscle.UsedHeads, muscle.AddHead, muscle.RemoveHead);
                SelectedPrimaryMusclesHeads.Add(muscle.MuscleGroup, muscleHeads);
            }
            
            SelectedSecondaryMusclesHeads = new Dictionary<MuscleGroup, IEnumerable<MuscleHead>>();
            
            foreach (var muscle in exercise.SecondaryMuscles)
            {
                var muscleHeads = new ObservedCollection<MuscleHead>(muscle.UsedHeads, muscle.AddHead, muscle.RemoveHead);
                SelectedSecondaryMusclesHeads.Add(muscle.MuscleGroup, muscleHeads);
            }
            
            SelectedPrimaryMuscleGroups = new ObservedCollection<MuscleGroup>(exercise.PrimaryMuscles.Select(GetMuscleGroup),
                muscleGroup =>
                {
                    var muscle = new ExercisedMuscle(muscleGroup);
                    exercise.AddPrimaryMuscle(muscle);
                        
                    var muscleHeads = new ObservedCollection<MuscleHead>(muscle.UsedHeads, muscle.AddHead, muscle.RemoveHead);
                    SelectedPrimaryMusclesHeads.Add(muscleGroup, muscleHeads);
                },
                muscleGroup =>
                {
                    exercise.RemovePrimaryMuscle(muscleGroup);
                    SelectedPrimaryMusclesHeads.Remove(muscleGroup);
                });
            
            SelectedSecondaryMuscleGroups = new ObservedCollection<MuscleGroup>(exercise.SecondaryMuscles.Select(GetMuscleGroup),
                muscleGroup =>
                {
                    var muscle = new ExercisedMuscle(muscleGroup);
                    exercise.AddSecondaryMuscle(muscle);
                        
                    var muscleHeads = new ObservedCollection<MuscleHead>(muscle.UsedHeads, muscle.AddHead, muscle.RemoveHead);
                    SelectedSecondaryMusclesHeads.Add(muscleGroup, muscleHeads);
                },
                muscleGroup =>
                {
                    exercise.RemoveSecondaryMuscle(muscleGroup);
                    SelectedSecondaryMusclesHeads.Remove(muscleGroup);
                });
            
           
            SelectedMotions = new ObservedCollection<JointMotion>(
                exercise.Motions,
                exercise.AddMotion,
                exercise.RemoveMotion
            );
            
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