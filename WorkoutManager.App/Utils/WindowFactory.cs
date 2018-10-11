using System.Windows;
using Unity;

namespace WorkoutManager.App.Utils
{
    internal class WindowFactory
    {
        private readonly IUnityContainer _container;

        public WindowFactory(IUnityContainer container)
        {
            _container = container;
        }

        public TWindow Get<TWindow>()
            where TWindow : Window
        {
            return _container.Resolve<TWindow>();
        }
    }
}