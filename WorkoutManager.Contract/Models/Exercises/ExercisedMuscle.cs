namespace WorkoutManager.Contract.Models.Exercises
{
    public class ExercisedMuscle
    {
        public Muscle Muscle { get; set; }
        
        public double RelativeEngagement { get; set; }

        public override string ToString() => Muscle?.Name;
 
        public ExercisedMuscle Clone() => new ExercisedMuscle
        {
            Muscle = Muscle.Clone(),
            RelativeEngagement = RelativeEngagement,
        };
    }
}