using System.Windows;
using WorkoutManager.App.Pages;

namespace WorkoutManager.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = new MainWindowViewModel();
        }
    }
}
