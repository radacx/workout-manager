using WorkoutManager.Contract.Models.Misc;

namespace WorkoutManager.Contract.Models.ExerciseSet
{
    public interface IExerciseSet : IEntity
    {
        double Weight { get; set; }
    }
}