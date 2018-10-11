using WorkoutManager.App.Structures;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.Pages.Motions.Models
{
    internal class JointMotionDialogViewModel : IViewModel
    {
        public JointMotion Motion { get; set; }
        
        public string SaveButtonTitle { get; set; }
    }
}