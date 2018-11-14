using System.Windows;
using System.Windows.Controls;
using WorkoutManager.App.Pages.Progress.Dialogs;

namespace WorkoutManager.App.Pages.Progress
{
    internal class ProgressPageDialogsSelector : DataTemplateSelector
    {
        public const string ProgressFilterDialog = "ProgressPage.ProgressFilterDialog";
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item) {
                case ProgressFilterDialogViewModel _:

                    return Application.Current.MainWindow?.FindResource(ProgressFilterDialog) as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }    
    }
}