using System.Windows;

namespace WorkoutManager.App.Pages.TrainingLog.Dialogs
{
    public partial class ExerciseSetDialog : Window
    {
        public ExerciseSetDialog()
        {
            InitializeComponent();
        }

        private void Submit_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}