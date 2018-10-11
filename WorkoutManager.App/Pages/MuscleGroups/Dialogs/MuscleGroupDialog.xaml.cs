using System.Windows;

namespace WorkoutManager.App.Pages.MuscleGroups.Dialogs
{
    internal partial class MuscleGroupDialog : Window
    {
        public MuscleGroupDialog()
        {
            InitializeComponent();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
