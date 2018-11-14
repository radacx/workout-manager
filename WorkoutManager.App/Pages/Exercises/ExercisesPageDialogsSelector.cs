using System.Windows;
using System.Windows.Controls;
using WorkoutManager.App.Pages.Exercises.Dialogs.ExerciseDialog;

namespace WorkoutManager.App.Pages.Exercises
{
    internal class ExercisesPageDialogsSelector : DataTemplateSelector
    {
        public const string ExerciseDialog = "ExercisesPage.ExerciseDialog";
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item) {
                case ExerciseDialogViewModel _:

                    return Application.Current.MainWindow?.FindResource(ExerciseDialog) as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}