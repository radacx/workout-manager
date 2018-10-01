using System;
using WorkoutManager.Contract.Models.Misc;

namespace WorkoutManager.Contract.Models.User
{
    public class UserPreferences : IEntity, IEquatable<UserPreferences>
    {
        public WeightUnits WeightUnits { get; set; }

        public bool Equals(IEntity other) => other is UserPreferences preferences && Equals(preferences);

        public int Id { get; set; }

        public bool Equals(UserPreferences other)
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

            return obj.GetType() == GetType() && Equals((UserPreferences) obj);
        }

        public override int GetHashCode() => Id;
    }
}