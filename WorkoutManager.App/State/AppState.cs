using System.Collections.Generic;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.State
{
    internal class AppState
    {
        public IEnumerable<JointMotion> Motions { get; set; }
    }
}