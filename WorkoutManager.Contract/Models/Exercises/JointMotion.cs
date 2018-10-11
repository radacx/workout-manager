using System;
using WorkoutManager.Contract.Models.Misc;

namespace WorkoutManager.Contract.Models.Exercises
{
    public class JointMotion : IEntity, IEquatable<JointMotion>
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public JointMotion() { }

        public JointMotion(string name)
        {
            Name = name;
        }

        public bool Equals(JointMotion other)
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

        public bool Equals(IEntity other) => other is JointMotion motion && Equals(motion);

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

            return obj.GetType() == GetType() && Equals((JointMotion) obj);
        }

        public override int GetHashCode() => Id;
    }
}