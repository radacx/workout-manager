using System;

namespace WorkoutManager.Contract.Models.Categories
{
    public class EntityReference : IEquatable<EntityReference>
    {
        public int Id { get; set; }

        public EntityReference() {}
        
        public EntityReference(int id)
        {
            Id = id;
        }

        public EntityReference Clone() => new EntityReference(Id);

        public bool Equals(EntityReference other)
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

            return obj.GetType() == GetType() && Equals((EntityReference) obj);
        }

        public override int GetHashCode() => Id;
    }
}