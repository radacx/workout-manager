using System.Windows;
using System.Windows.Controls;
using WorkoutManager.App.Pages.TrainingLog.Dialogs.ExerciseSetDialog.Dynamic;
using WorkoutManager.App.Pages.TrainingLog.Dialogs.ExerciseSetDialog.Isometric;

namespace WorkoutManager.App.Pages.TrainingLog.Dialogs.ExerciseSetDialog
{
    internal class ExerciseSetDialogTemplateSelector : DataTemplateSelector
    {
        public const string DynamicSet = "ExerciseSetDialog.DynamicSet";
        public const string IsometricSet = "ExerciseSetDialog.IsometricSet";
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item) {
                case DynamicExerciseSetViewModel _:

                    return Application.Current.MainWindow?.FindResource(DynamicSet) as DataTemplate;
                case IsometricExerciseSetViewModel _:

                    return Application.Current.MainWindow?.FindResource(IsometricSet) as DataTemplate; 
            }

            return base.SelectTemplate(item, container);
        }
    }
}