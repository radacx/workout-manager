using System;
using WorkoutManager.Contract.Structures;

namespace WorkoutManager.Contract.Models.Base
{
    public abstract class Entity : PropertyChangedNotifier, IEntity, IEquatable<Entity>
    {
        public int Id { get; set; }

        public bool Equals(Entity other)
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

        public bool Equals(IEntity other) => other is Entity entity && Equals(entity);

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

            return obj.GetType() == GetType() && Equals((Entity) obj);
        }

        public override int GetHashCode() => Id;

        public abstract IEntity GenericClone();
    }
}