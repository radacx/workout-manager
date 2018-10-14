using System;
using WorkoutManager.Contract.Models.Misc;

namespace WorkoutManager.Contract.Models.ExerciseSet
{
    public class IsometricExerciseSet : IExerciseSet, IEquatable<IsometricExerciseSet>
    {
        public int Id { get; set; }
        
        public double Weight { get; set; }
        
        public TimeSpan Duration { get; set; }

        public bool Equals(IsometricExerciseSet other)
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

        public bool Equals(IEntity other) => other is IsometricExerciseSet set && Equals(set);

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

            return obj.GetType() == GetType() && Equals((IsometricExerciseSet) obj);
        }

        public override int GetHashCode() => Id;

        public override string ToString() => $"{Duration} @ {Weight}";
    }
}