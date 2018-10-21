using System.Collections.Generic;
using System.Linq;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Sessions;

namespace WorkoutManager.App.Pages.Progress.Structures.Calculators
{
    internal class MuscleMetricCalculator : MetricsCalculator
    {
        private readonly IEnumerable<Muscle> _muscles;

        public MuscleMetricCalculator(IEnumerable<Muscle> muscles)
        {
            _muscles = muscles;
        }

        private double GetMultiplier(SessionExercise sessionExercise)
        {
            var exercisedMuscles =
                sessionExercise.Exercise.Muscles.Where(
                    exercisedMuscle => _muscles.Contains(exercisedMuscle.Muscle)
                );

            var multiplier = exercisedMuscles.Aggregate(
                0d,
                (totalMultiplier, exercisedMuscle) => totalMultiplier + exercisedMuscle.RelativeEngagement / 100.0
            );

            return multiplier;
        }

        protected override double GetSetCount(SessionExercise sessionExercise)
            => base.GetSetCount(sessionExercise) * GetMultiplier(sessionExercise);

        protected override double GetLoadVolume(SessionExercise sessionExercise, double bodyweight)
            => base.GetLoadVolume(sessionExercise, bodyweight) * GetMultiplier(sessionExercise);
    }
}