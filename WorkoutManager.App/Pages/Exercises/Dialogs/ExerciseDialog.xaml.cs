using System.Windows;

namespace WorkoutManager.App.Pages.Exercises.Dialogs
{
    public partial class ExerciseDialog : Window
    {
        public ExerciseDialog()
        {
            InitializeComponent();
        }

        private void Submit_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
