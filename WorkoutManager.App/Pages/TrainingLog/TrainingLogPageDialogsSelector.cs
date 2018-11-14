using System.Windows;
using System.Windows.Controls;
using WorkoutManager.App.Pages.TrainingLog.Dialogs.TrainingSessionDialog;

namespace WorkoutManager.App.Pages.TrainingLog
{
    internal class TrainingLogPageDialogsSelector : DataTemplateSelector
    {
        public const string TrainingSessionDialog = "TrainingLogPage.TrainingSessionDialog";
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item) {
                case TrainingSessionDialogViewModel _:

                    return Application.Current.MainWindow?.FindResource(TrainingSessionDialog) as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}