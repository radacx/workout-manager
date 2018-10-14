namespace WorkoutManager.Contract.Models.Progress
{
    public class ProgressResult
    {
        public string DateUnit { get; set; }
        
        public string Value { get; set; }

        public override string ToString() => $"{DateUnit}: {Value}";
    }
}