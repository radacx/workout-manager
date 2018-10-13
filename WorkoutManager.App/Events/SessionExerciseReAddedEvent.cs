using WorkoutManager.Contract.Models.Sessions;

namespace WorkoutManager.App.Events
{
    internal class SessionExerciseReAddedEvent
    {
        public SessionExerciseReAddedEvent(SessionExercise sessionExercise)
        {
            SessionExercise = sessionExercise;
        }

        public SessionExercise SessionExercise{ get; }
    }
}