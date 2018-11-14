using System.Windows;
using System.Windows.Controls;
using WorkoutManager.App.Pages.Exercises.Dialogs.AddMuscleDialog;
using WorkoutManager.App.Pages.Exercises.Dialogs.EditMuscleDialog;

namespace WorkoutManager.App.Pages.Exercises.Dialogs.ExerciseDialog
{
    internal class ExerciseDialogDialogsSelector : DataTemplateSelector
    {
        public const string AddExercisedMuscleDialog = "ExerciseDialog.AddExercisedMuscleDialog";
        public const string EditExercisedMuscleDialog = "ExerciseDialog.EditExercisedMuscleDialog";
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item) {
                case AddExercisedMuscleDialogViewModel _:

                    return Application.Current.MainWindow?.FindResource(AddExercisedMuscleDialog) as DataTemplate;
                case EditExercisedMuscleDialogViewModel _:

                    return Application.Current.MainWindow?.FindResource(EditExercisedMuscleDialog) as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}