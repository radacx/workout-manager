using System.Collections.Generic;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.Events
{
    public class ExercisesChangedEvent
    {
        public ExercisesChangedEvent(IEnumerable<Exercise> actualExercises)
        {
            ActualExercises = actualExercises;
        }

        public IEnumerable<Exercise> ActualExercises { get; }
    }
}