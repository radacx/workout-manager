using System.Windows;
using System.Windows.Controls;
using WorkoutManager.App.Pages.TrainingLog.Dialogs.SessionExerciseDialog;

namespace WorkoutManager.App.Pages.TrainingLog.Dialogs.TrainingSessionDialog
{
    internal class TrainingSessionDialogDialogsSelector : DataTemplateSelector
    {
        public const string ExerciseDialog = "TrainingSessionDialog.ExerciseDialog";
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item) {
                case SessionExerciseDialogViewModel _:

                    return Application.Current.MainWindow?.FindResource(ExerciseDialog) as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}