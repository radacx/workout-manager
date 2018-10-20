using WorkoutManager.Contract.Models.Progress;

namespace WorkoutManager.App.Events
{
    internal class ProgressFilterSelectedEvent
    {
        public ProgressFilterSelectedEvent(ProgressFilter filter)
        {
            Filter = filter;
        }

        public ProgressFilter Filter { get; }
    }
}