using System.Windows;

namespace WorkoutManager.App.Pages.Categories.Dialogs
{
    internal partial class CategoryDialog : Window
    {
        public CategoryDialog()
        {
            InitializeComponent();
        }

        private void Submit_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
