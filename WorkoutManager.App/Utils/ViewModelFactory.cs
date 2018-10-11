using Unity;
using WorkoutManager.App.Structures;

namespace WorkoutManager.App.Utils
{
    internal class ViewModelFactory<TViewModel>
        where TViewModel : IViewModel
    {
        private readonly IUnityContainer _container;

        public ViewModelFactory(IUnityContainer container)
        {
            _container = container;
        }

        public TViewModel Get()       
        {
            return _container.Resolve<TViewModel>();
        }
    }
}