using System.ComponentModel;

namespace WorkoutManager.App.Pages.Progress.Structures
{
    internal enum GroupBy
    {
        [Description("Day")]
        Day,
            
        [Description("Week")]
        Week,
            
        [Description("1 Month")]
        Month1,
            
        [Description("3 Months")]
        Month3,
            
        [Description("6 Months")]
        Month6,
            
        [Description("Year")]
        Year,
    }
}