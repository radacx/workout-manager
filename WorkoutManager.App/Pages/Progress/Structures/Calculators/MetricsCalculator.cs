using System.Linq;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Contract.Models.Sessions.Sets;

namespace WorkoutManager.App.Pages.Progress.Structures.Calculators
{
    internal class MetricsCalculator : IMetricsCalculator
    {
        protected virtual double GetSetCount(SessionExercise sessionExercise) => sessionExercise.Sets.Length;

        protected virtual double GetLoadVolume(SessionExercise sessionExercise, double bodyweight)
        {
            var bodyweightVolume = sessionExercise.Exercise.RelativeBodyweight / 100 * bodyweight;
            var dynamicSets = sessionExercise.Sets.OfType<DynamicExerciseSet>();

            var exerciseVolume = dynamicSets.Aggregate(
                0d,
                (totalVolume, set) => totalVolume + set.Reps * (set.Weight + bodyweightVolume)
            );

            return exerciseVolume;
        }
        
        public double GetSetCount(TrainingSession session)
            => session.Exercises.Aggregate(
                0d,
                (totalCount, exercise) => totalCount + GetSetCount(exercise)
            );
        
        public double GetLoadVolume(TrainingSession session)
            => session.Exercises.Aggregate(
                0d,
                (totalVolume, exercise) => totalVolume + GetLoadVolume(exercise, session.Bodyweight)
            );
    }
}