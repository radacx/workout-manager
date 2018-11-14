using System.Windows;
using System.Windows.Controls;
using WorkoutManager.App.Pages.Categories.Dialogs.AddCategoryOptionsDialog;

namespace WorkoutManager.App.Pages.Categories.Dialogs.CategoryDialog
{
    internal class CategoryDialogDialogsSelector : DataTemplateSelector
    {
        public const string AddOptionsDialog = "CategoryDialog.AddOptionsDialog";
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item) {
                case AddCategoryOptionsDialogViewModel _:

                    return Application.Current.MainWindow?.FindResource(AddOptionsDialog) as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}