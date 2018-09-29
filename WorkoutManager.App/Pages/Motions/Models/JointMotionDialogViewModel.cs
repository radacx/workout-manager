using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.Pages.Motions.Models
{
    internal class JointMotionDialogViewModel
    {
        public JointMotion Motion { get; }
        
        public string SaveButtonTitle { get; set; }

        public JointMotionDialogViewModel(JointMotion motion)
        {
            Motion = motion;
        }
    }
}