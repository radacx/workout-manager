using System.Windows;
using System.Windows.Controls;
using WorkoutManager.App.Pages.Categories.Dialogs.CategoryDialog;

namespace WorkoutManager.App.Pages.Categories
{
    internal class CategoryPageDialogsSelector : DataTemplateSelector
    {
        public const string CategoryDialog = "CategoryPage.CategoryDialog";
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item) {
                case CategoryDialogViewModel _:

                    return Application.Current.MainWindow?.FindResource(CategoryDialog) as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}