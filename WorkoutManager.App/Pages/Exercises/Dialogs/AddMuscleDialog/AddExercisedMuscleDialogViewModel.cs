using System.Collections.Generic;
using WorkoutManager.App.Pages.Exercises.Dialogs.Models;
using WorkoutManager.App.Structures.Dialogs;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.Pages.Exercises.Dialogs.AddMuscleDialog
{
    internal class AddExercisedMuscleDialogViewModel : DialogModelBase
    {
        public ExercisedMuscleViewModel ExercisedMuscle { get; set; }

        public IEnumerable<Muscle> AvailableMuscles { get; set; }
    }
}