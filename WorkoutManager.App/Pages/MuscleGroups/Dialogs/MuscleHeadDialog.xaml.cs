using System.Windows;

namespace WorkoutManager.App.Pages.MuscleGroups.Dialogs
{
    internal partial class MuscleHeadDialog : Window
    {
        public MuscleHeadDialog()
        {
            InitializeComponent();
        }

        private void Submit_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
