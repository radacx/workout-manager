using WorkoutManager.App.Structures;
using WorkoutManager.Contract.Models.ExerciseSet;

namespace WorkoutManager.App.Pages.TrainingLog.Models
{
    internal class ExerciseSetDialogViewModel : IViewModel
    {
        public IExerciseSet ExerciseSet { get; set; }

        public string SaveButtonTitle { get; set; }
    }
}