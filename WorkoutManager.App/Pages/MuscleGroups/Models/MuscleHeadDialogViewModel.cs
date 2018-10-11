using WorkoutManager.App.Structures;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.Pages.MuscleGroups.Models
{
    internal class MuscleHeadDialogViewModel : IViewModel
    {
        public MuscleHead MuscleHead { get; set; }
        
        public string SaveButtonTitle { get; set; }
    }
}