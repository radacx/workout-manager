using WorkoutManager.Contract.Models.Base;

namespace WorkoutManager.Contract.Models.User
{
    public class UserPreferences : Entity
    {
        public WeightUnits WeightUnits { get; set; }

        public override IEntity GenericClone() =>  new UserPreferences
        {
            Id = Id,
            WeightUnits = WeightUnits,
        };

        public UserPreferences Clone() => GenericClone() as UserPreferences;
    }
}