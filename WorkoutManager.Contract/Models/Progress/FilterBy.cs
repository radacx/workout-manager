using System.ComponentModel;

namespace WorkoutManager.Contract.Models.Progress
{
    public enum FilterBy
    {
        [Description("Muscle")]
        Muscle,
        
        [Description("Exercise")]
        Exercise,
        
        [Description("Category")]
        Category,
    }
}