using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.Pages.MuscleGroups.Models
{
    internal class MuscleHeadDialogViewModel
    {
        public MuscleHead Head { get; }
        
        public string SaveButtonTitle { get; set; }

        public MuscleHeadDialogViewModel(MuscleHead head)
        {
            Head = head;
        }
    }
}