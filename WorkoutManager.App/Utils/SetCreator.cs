using System;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.ExerciseSet;

namespace WorkoutManager.App.Utils
{
    internal class SetCreator
    {
        public static IExerciseSet Create(ContractionType type)
        {
            switch (type)
            {
                case ContractionType.Dynamic:
                    return new DynamicExerciseSet();
                
                case ContractionType.Isometric:
                    return new IsometricExerciseSet();
                
                default:
                    throw new ArgumentException($"Invalid contraction type: {type}");
            }
        }
    }
}