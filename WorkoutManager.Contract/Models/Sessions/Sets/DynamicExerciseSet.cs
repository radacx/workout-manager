namespace WorkoutManager.Contract.Models.Sessions.Sets
{
    public class DynamicExerciseSet : ExerciseSet
    {
        public int Reps { get; set; }

        public override string ToString() => $"{Reps} @ {Weight}";

        public override ExerciseSet Clone() => new DynamicExerciseSet
        {
            Reps = Reps,
            Weight = Weight,
        };
    }
}