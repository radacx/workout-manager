using System.Windows;

namespace WorkoutManager.App.Pages.TrainingLog.Dialogs
{
    public partial class TrainingSessionDialog : Window
    {
        public TrainingSessionDialog()
        {
            InitializeComponent();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
