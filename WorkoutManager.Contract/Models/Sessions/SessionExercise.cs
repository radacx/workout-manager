using System.Collections.Generic;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.ExerciseSet;

namespace WorkoutManager.Contract.Models.Sessions
{
    public class SessionExercise
    {
        public ICollection<IExerciseSet> Sets { get; set; } = new List<IExerciseSet>();
        
        public Exercise Exercise { get; set; }
    }
}