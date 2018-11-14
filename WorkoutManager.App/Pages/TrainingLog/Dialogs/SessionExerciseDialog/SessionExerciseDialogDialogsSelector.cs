using System.Windows;
using System.Windows.Controls;
using WorkoutManager.App.Pages.TrainingLog.Dialogs.ExerciseSetDialog;

namespace WorkoutManager.App.Pages.TrainingLog.Dialogs.SessionExerciseDialog
{
    internal class SessionExerciseDialogDialogsSelector : DataTemplateSelector
    {
        public const string ExerciseSetDialog = "SessionExerciseDialog.ExerciseSetDialog";
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item) {
                case ExerciseSetDialogViewModel _:

                    return Application.Current.MainWindow?.FindResource(ExerciseSetDialog) as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}