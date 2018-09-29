using WorkoutManager.Contract.Models.ExerciseSet;

namespace WorkoutManager.App.Pages.TrainingLog.Models
{
    internal class ExerciseSetDialogViewModel
    {
        public IExerciseSet ExerciseSet { get; set; }

        public ExerciseSetDialogViewModel(IExerciseSet exerciseSet)
        {
            ExerciseSet = exerciseSet;
        }
    }
}