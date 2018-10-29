using System;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Exercises.Sets;

namespace WorkoutManager.App.Utils
{
    internal static class SetCreator
    {
        public static ExerciseSet Create(ContractionType type)
        {
            switch (type)
            {
                case ContractionType.Dynamic:
                    return new DynamicExerciseSet
                    {
                        Reps = 1
                    };
                
                case ContractionType.Isometric:

                    return new IsometricExerciseSet
                    {
                        Duration = TimeSpan.FromSeconds(1)
                    };
                
                default:
                    throw new ArgumentException($"Invalid contraction type: {type}");
            }
        }
    }
}