using System.ComponentModel;

namespace WorkoutManager.App.Pages.Progress.Structures
{
    internal enum FilterBy
    {
        [Description("Muscle")]
        Muscle,
        
        [Description("Exercise")]
        Exercise,
        
        [Description("Category")]
        Category,
    }
}