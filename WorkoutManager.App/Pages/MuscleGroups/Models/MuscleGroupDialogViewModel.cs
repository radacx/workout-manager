using WorkoutManager.App.Structures;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.Pages.MuscleGroups.Models
{
    internal class MuscleGroupDialogViewModel : IViewModel
    {
        public MuscleGroup MuscleGroup { get; set; }
        
        public string SaveButtonTitle { get; set; } 
    }
}