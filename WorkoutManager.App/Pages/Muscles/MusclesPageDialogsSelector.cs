using System.Windows;
using System.Windows.Controls;
using WorkoutManager.App.Pages.Muscles.Dialogs.MuscleDialog;

namespace WorkoutManager.App.Pages.Muscles
{
    internal class MusclesPageDialogsSelector : DataTemplateSelector
    {
        public const string MuscleDialog = "MusclesPage.MuscleDialog";
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item) {
                case MuscleDialogViewModel _:

                    return Application.Current.MainWindow?.FindResource(MuscleDialog) as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}