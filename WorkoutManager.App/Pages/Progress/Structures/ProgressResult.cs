namespace WorkoutManager.App.Pages.Progress.Structures
{
    internal class ProgressResult
    {
        public string DateUnit { get; set; }
        
        public string Value { get; set; }

        public override string ToString() => $"{DateUnit}: {Value}";
    }
}