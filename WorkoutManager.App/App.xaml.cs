using System;
using System.Windows;
using Unity;
using WorkoutManager.App.Config;
using WorkoutManager.App.Pages;

namespace WorkoutManager.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var container = new UnityContainer();
            Bootstrap.Register(container);
            var mainWindow = container.Resolve<MainWindow>();

            if (mainWindow == null)
            {
                throw new Exception("Main window was not resolved");
            }
            
            Current.MainWindow = mainWindow;
            Current.MainWindow.Show();
        }
    }
}
