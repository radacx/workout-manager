using WorkoutManager.App.Structures.ViewModels;

namespace WorkoutManager.App.Structures.Dialogs
{
    internal abstract class DialogModelBase : ViewModelBase
    {
        public string SubmitButtonTitle { get; set; }
        
        public string DialogTitle { get; set; }
    }
}