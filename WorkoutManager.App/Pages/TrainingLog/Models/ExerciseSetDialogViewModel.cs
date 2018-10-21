using System.Windows;
using System.Windows.Controls;
using WorkoutManager.App.Structures;
using WorkoutManager.Contract.Models.ExerciseSet;

namespace WorkoutManager.App.Pages.TrainingLog.Models
{
    internal class ExerciseSetDialogSelector : DataTemplateSelector
    {
        public const string DynamicSetDialogDataTemplateIdentifier = "DynamicSetDialogDataTemplateIdentifier";
        public const string IsometricSetDialogDataTemplateIdentifier = "IsometricSetDialogDataTemplateIdentifier";
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item) {
                case DynamicExerciseSet _:

                    return Application.Current.MainWindow?.FindResource(DynamicSetDialogDataTemplateIdentifier) as DataTemplate;
                case IsometricExerciseSet _:

                    return Application.Current.MainWindow?.FindResource(IsometricSetDialogDataTemplateIdentifier) as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
    
    internal class ExerciseSetDialogViewModel : DialogModelBase
    {
        public IExerciseSet ExerciseSet { get; set; }
    }
}