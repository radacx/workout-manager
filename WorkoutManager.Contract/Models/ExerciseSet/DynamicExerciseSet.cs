namespace WorkoutManager.Contract.Models.ExerciseSet
{
    public class DynamicExerciseSet : IExerciseSet
    {
        public double Weight { get; set; }
        
        public double Reps { get; set; }
    }
}