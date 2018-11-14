namespace WorkoutManager.Contract.Models.Sessions.Sets
{
    public abstract class ExerciseSet
    {
        public double Weight { get; set; }

        public abstract ExerciseSet Clone();
    }
}