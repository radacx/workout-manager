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
        
        private List<MuscleGroup> _primaryMuscles = new List<MuscleGroup>();
        private List<MuscleGroup> _secondaryMuscles = new List<MuscleGroup>();
        private List<JointMotion> _motions = new List<JointMotion>();
        
        public MuscleGroup[] PrimaryMuscles
        {
            get => _primaryMuscles.ToArray();
            set => _primaryMuscles = value.ToList();
        }

        public MuscleGroup[] SecondaryMuscles
        {
            get => _secondaryMuscles.ToArray();
            set => _secondaryMuscles = value.ToList();
        }

        public JointMotion[] Motions { 
            get => _motions.ToArray();
            set => _motions = value.ToList();
        }
        
        public Exercise()
        {
            ContractionType = ContractionType.Dynamic;
        }

        public void AddPrimaryMuscle(MuscleGroup muscle) => _primaryMuscles.Add(muscle);

        public void AddSecondaryMuscle(MuscleGroup muscle) => _secondaryMuscles.Add(muscle);

        public void RemovePrimaryMuscle(MuscleGroup muscleGroup) => _primaryMuscles.Remove(muscleGroup);

        public void RemoveSecondaryMuscle(MuscleGroup muscleGroup) => _secondaryMuscles.Remove(muscleGroup);

        public void AddMotion(JointMotion motion) => _motions.Add(motion);

        public void RemoveMotion(JointMotion motion) => _motions.Remove(motion);
        
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