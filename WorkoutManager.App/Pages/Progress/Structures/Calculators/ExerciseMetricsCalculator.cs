using System.Collections.Generic;
using System.Linq;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Sessions;

namespace WorkoutManager.App.Pages.Progress.Structures.Calculators
{
    internal class ExerciseMetricsCalculator : MetricsCalculator
    {
        private readonly IEnumerable<Exercise> _exercises;

        public ExerciseMetricsCalculator(IEnumerable<Exercise> exercises)
        {
            _exercises = exercises;
        }

        private bool ShouldBeCounted(SessionExercise sessionExercise) => _exercises.Contains(sessionExercise.Exercise);

        protected override double GetSetCount(SessionExercise sessionExercise)
            => ShouldBeCounted(sessionExercise) ? base.GetSetCount(sessionExercise) : 0;

        protected override double GetLoadVolume(SessionExercise sessionExercise, double bodyweight)
            => ShouldBeCounted(sessionExercise) ? base.GetLoadVolume(sessionExercise, bodyweight) : 0;
    }
}