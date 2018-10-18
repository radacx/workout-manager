using System;
using WorkoutManager.Contract.Models.Misc;

namespace WorkoutManager.Contract.Models.Exercises
{
    public class MuscleGroup : IEntity, IEquatable<MuscleGroup>
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public bool Equals(MuscleGroup other)
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

        public bool Equals(IEntity other) => other is MuscleGroup muscle && Equals(muscle);

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

            return obj.GetType() == GetType() && Equals((MuscleGroup) obj);
        }

        public override int GetHashCode() => Id;

        public override string ToString() => Name;
    }
}