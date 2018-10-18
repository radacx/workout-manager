using System.Windows;

namespace WorkoutManager.App.Pages.Muscles.Dialogs
{
    internal partial class MuscleDialog : Window
    {
        public MuscleDialog()
        {
            InitializeComponent();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
