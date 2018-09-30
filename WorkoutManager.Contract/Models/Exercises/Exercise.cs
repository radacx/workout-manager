using System;
using System.Collections.Generic;
using System.Linq;
using WorkoutManager.Contract.Models.Misc;

namespace WorkoutManager.Contract.Models.Exercises
{
    public class Exercise : IEntity, IEquatable<Exercise>
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public ContractionType ContractionType { get; set; }

        public double RelativeBodyweight { get; set; }
        
        private List<ExercisedMuscle> _primaryMuscles = new List<ExercisedMuscle>();
        private List<ExercisedMuscle> _secondaryMuscles = new List<ExercisedMuscle>();

        public IEnumerable<ExercisedMuscle> PrimaryMuscles
        {
            get => _primaryMuscles;
            set => _primaryMuscles = value.ToList();
        }

        public IEnumerable<ExercisedMuscle> SecondaryMuscles
        {
            get => _secondaryMuscles;
            set => _secondaryMuscles = value.ToList();
        }

        public IEnumerable<JointMotion> Motions { get; set; } = new List<JointMotion>();
        
        public Exercise()
        {
            ContractionType = ContractionType.Dynamic;
        }

        public void AddPrimaryMuscle(ExercisedMuscle muscle) => _primaryMuscles.Add(muscle);

        public void AddSecondaryMuscle(ExercisedMuscle muscle) => _secondaryMuscles.Add(muscle);

        public void RemovePrimaryMuscle(MuscleGroup muscleGroup)
            => _primaryMuscles.RemoveAll(muscle => muscle.MuscleGroup.Equals(muscleGroup));
        
        public void RemoveSecondaryMuscle(MuscleGroup muscleGroup)
            => _secondaryMuscles.RemoveAll(muscle => muscle.MuscleGroup.Equals(muscleGroup));
        
        public bool Equals(Exercise other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id == other.Id;
        }

        public bool Equals(IEntity other) => other is Exercise exercise && Equals(exercise);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((Exercise) obj);
        }

        public override int GetHashCode() => Id;

        public override string ToString() => Name;
    }
}