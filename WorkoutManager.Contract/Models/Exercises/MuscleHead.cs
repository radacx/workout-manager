using System;
using WorkoutManager.Contract.Models.Misc;

namespace WorkoutManager.Contract.Models.Exercises
{
    public class MuscleHead : IEntity, IEquatable<MuscleHead>
    {
        public MuscleHead() {}
        
        public MuscleHead(string name)
        {
            Name = name;
        }

        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public bool Equals(MuscleHead other)
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

        public bool Equals(IEntity other) => other is MuscleHead muscleHead && Equals(muscleHead);

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

            return obj.GetType() == GetType() && Equals((MuscleHead) obj);
        }

        public override int GetHashCode() => Id;
    }
}