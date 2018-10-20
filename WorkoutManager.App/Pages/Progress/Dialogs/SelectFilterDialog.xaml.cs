using System.Windows;

namespace WorkoutManager.App.Pages.Progress.Dialogs
{
    internal partial class SelectFilterDialog : Window
    {
        public SelectFilterDialog()
        {
            InitializeComponent();
        }

        private void Submit_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
