using System.Windows;

namespace WorkoutManager.App.TrainingLog
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
