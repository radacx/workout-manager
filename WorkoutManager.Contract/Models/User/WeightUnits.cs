using System.ComponentModel;

namespace WorkoutManager.Contract.Models.User
{
    public enum WeightUnits
    {
        [Description("kg")]
        Kilograms,
        
        [Description("lbs")]
        Pounds,
    }
}