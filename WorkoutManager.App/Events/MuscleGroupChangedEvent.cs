using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.Events
{
    internal class MuscleGroupChangedEvent
    {
        public MuscleGroupChangedEvent(MuscleGroup muscleGroup)
        {
            MuscleGroup = muscleGroup;
        }

        public MuscleGroup MuscleGroup { get; }
    }
}