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

        public List<MuscleGroup> MuscleGroups { get; } = new List<MuscleGroup>();

        public List<JointMotion> Motions { get; } = new List<JointMotion>();
        
        public Func<object, string> GetJointMotionText { get; }
        
        public Func<object, string> GetMuscleGroupText { get; }
        
        public Func<object, string> GetMuscleHeadText { get; }
        
        public IEnumerable<JointMotion> SelectedMotions { get; set; }

        public IEnumerable<MuscleGroup> SelectedPrimaryMuscleGroups { get; set; }

        public IEnumerable<MuscleGroup> SelectedSecondaryMuscleGroups { get; set; }

        public Dictionary<MuscleGroup, IEnumerable<MuscleHead>> SelectedPrimaryMusclesHeads { get; set; }

        public Dictionary<MuscleGroup, IEnumerable<MuscleHead>> SelectedSecondaryMusclesHeads { get; set; }

        private static MuscleGroup GetMuscleGroup(ExercisedMuscle muscle) => muscle.MuscleGroup;
        
        public ExerciseDialogViewModel(Repository<JointMotion> motionsRepository, Repository<MuscleGroup> muscleGroupRepository)
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

                SelectedPrimaryMusclesHeads = new Dictionary<MuscleGroup, IEnumerable<MuscleHead>>();
            
                foreach (var muscle in Exercise.PrimaryMuscles)
                {
                    var muscleHeads = new ObservedCollection<MuscleHead>(muscle.UsedHeads, muscle.AddHead, muscle.RemoveHead);
                    SelectedPrimaryMusclesHeads.Add(muscle.MuscleGroup, muscleHeads);
                }
                
                SelectedSecondaryMusclesHeads = new Dictionary<MuscleGroup, IEnumerable<MuscleHead>>();

                foreach (var muscle in Exercise.SecondaryMuscles)
                {
                    var muscleHeads = new ObservedCollection<MuscleHead>(muscle.UsedHeads, muscle.AddHead, muscle.RemoveHead);
                    SelectedSecondaryMusclesHeads.Add(muscle.MuscleGroup, muscleHeads);
                }
                
                SelectedPrimaryMuscleGroups = new ObservedCollection<MuscleGroup>(
                    Exercise.PrimaryMuscles.Select(GetMuscleGroup),
                    muscleGroup =>
                    {
                        var muscle = new ExercisedMuscle(muscleGroup);
                        Exercise.AddPrimaryMuscle(muscle);

                        var muscleHeads = new ObservedCollection<MuscleHead>(
                            muscle.UsedHeads,
                            muscle.AddHead,
                            muscle.RemoveHead
                        );

                        SelectedPrimaryMusclesHeads.Add(muscleGroup, muscleHeads);
                    },
                    muscleGroup =>
                    {
                        Exercise.RemovePrimaryMuscle(muscleGroup);
                        SelectedPrimaryMusclesHeads.Remove(muscleGroup);
                    }
                );
                
                SelectedSecondaryMuscleGroups = new ObservedCollection<MuscleGroup>(
                    Exercise.SecondaryMuscles.Select(GetMuscleGroup),
                    muscleGroup =>
                    {
                        var muscle = new ExercisedMuscle(muscleGroup);
                        Exercise.AddSecondaryMuscle(muscle);

                        var muscleHeads = new ObservedCollection<MuscleHead>(
                            muscle.UsedHeads,
                            muscle.AddHead,
                            muscle.RemoveHead
                        );

                        SelectedSecondaryMusclesHeads.Add(muscleGroup, muscleHeads);
                    },
                    muscleGroup =>
                    {
                        Exercise.RemoveSecondaryMuscle(muscleGroup);
                        SelectedSecondaryMusclesHeads.Remove(muscleGroup);
                    }
                ); 
            };
            
            Task.Run(
                () =>
                {
                    Motions.AddRange(motionsRepository.GetAll().OrderBy(motion => motion.Name));
                    MuscleGroups.AddRange(muscleGroupRepository.GetAll().OrderBy(muscleGroup => muscleGroup.Name));
                    OnPropertyChanged(string.Empty);
                });
            
            GetJointMotionText = motion => (motion as JointMotion)?.Name;

            GetMuscleGroupText = muscleGroup => (muscleGroup as MuscleGroup)?.Name;

            GetMuscleHeadText = muscleHead => (muscleHead as MuscleHead)?.Name;
        }
    }
}