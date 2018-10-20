using WorkoutManager.App.Structures;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.Pages.Muscles.Models
{
    internal class MuscleDialogViewModel : ViewModelBase
    {
        public Muscle Muscle { get; set; }
        
        public string SaveButtonTitle { get; set; } 
    }
}