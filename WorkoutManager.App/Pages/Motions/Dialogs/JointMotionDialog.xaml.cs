using System.Windows;

namespace WorkoutManager.App.Pages.Motions.Dialogs
{
    public partial class JointMotionDialog : Window
    {
        public JointMotionDialog()
        {
            InitializeComponent();
        }

        private void Submit_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
