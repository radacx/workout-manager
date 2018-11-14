using System;

namespace WorkoutManager.Contract.Models.Sessions.Sets
{
    public class IsometricExerciseSet : ExerciseSet
    {
        public TimeSpan Duration { get; set; }

        public override string ToString() => $"{Duration} @ {Weight}";

        public override ExerciseSet Clone() => new IsometricExerciseSet
        {
            Weight = Weight,
            Duration = Duration,
        };
    }
}