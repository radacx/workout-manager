using System.ComponentModel;

namespace WorkoutManager.App.Pages.Progress.Structures
{
    internal enum FilterBy
    {
        [Description("Primary muscle group")]
        PrimaryMuscle,
            
        [Description("Secondary muscle group")]
        SecondaryMuscle,
        
        [Description("Exercise")]
        Exercise,
        
        [Description("Category")]
        Category,
    }
}