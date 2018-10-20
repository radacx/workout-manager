using System.Windows;

namespace WorkoutManager.App.Pages.Progress.Dialogs
{
    internal partial class ProgressFilterDialog : Window
    {
        public ProgressFilterDialog()
        {
            InitializeComponent();
        }

        private void Submit_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
