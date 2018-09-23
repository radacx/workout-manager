using System;

namespace WorkoutManager.Contract.Models.ExerciseSet
{
    public class IsometricExerciseSet : IExerciseSet
    {
        public double Weight { get; set; }
        
        public TimeSpan Duration { get; set; }
    }
}