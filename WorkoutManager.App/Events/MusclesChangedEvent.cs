using System.Collections.Generic;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.Events
{
    internal class MusclesChangedEvent
    {
        public MusclesChangedEvent(IEnumerable<Muscle> actualMuscles)
        {
            ActualMuscles = actualMuscles;
        }

        public IEnumerable<Muscle> ActualMuscles { get; }
    }
}