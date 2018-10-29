namespace WorkoutManager.Contract.Models.Exercises.Sets
{
    public class DynamicExerciseSet : ExerciseSet
    {
        public int Reps { get; set; }

        public override string ToString() => $"{Reps} @ {Weight}";
    }
}