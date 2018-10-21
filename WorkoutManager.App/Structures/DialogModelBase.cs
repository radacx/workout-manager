namespace WorkoutManager.App.Structures {
    internal abstract class DialogModelBase : ViewModelBase
    {
        public string SubmitButtonTitle { get; set; }
        
        public string DialogTitle { get; set; }
    }
}