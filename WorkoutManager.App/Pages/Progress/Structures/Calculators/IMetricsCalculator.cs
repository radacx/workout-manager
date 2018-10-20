using WorkoutManager.Contract.Models.Sessions;

namespace WorkoutManager.App.Pages.Progress.Structures.Calculators
{
    internal interface IMetricsCalculator
    {
        double GetSetCount(TrainingSession session);

        double GetLoadVolume(TrainingSession session);
    }
}