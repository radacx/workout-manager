using System;

namespace WorkoutManager.Contract.Models.Exercises.Sets
{
    public class IsometricExerciseSet : ExerciseSet
    {
        public TimeSpan Duration { get; set; }

        public override string ToString() => $"{Duration} @ {Weight}";
    }
}