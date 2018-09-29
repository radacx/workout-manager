using System;
using WorkoutManager.Contract.Models.Misc;

namespace WorkoutManager.Contract.Models.ExerciseSet
{
    public class DynamicExerciseSet : IExerciseSet, IEquatable<DynamicExerciseSet>
    {
        public int Id { get; set; }
        
        public double Weight { get; set; }
        
        public int Reps { get; set; }

        public bool Equals(DynamicExerciseSet other)
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

        public bool Equals(IEntity other) => other is DynamicExerciseSet set && Equals(set);

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

            return obj.GetType() == GetType() && Equals((DynamicExerciseSet) obj);
        }

        public override int GetHashCode() => Id;
    }
}