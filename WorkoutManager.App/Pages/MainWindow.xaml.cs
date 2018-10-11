using System.Windows;

namespace WorkoutManager.App.Pages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    internal partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            
            DataContext = vm;
        }
    }
}
