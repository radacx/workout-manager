using Unity;
using WorkoutManager.App.Structures;

namespace WorkoutManager.App.Utils
{
    internal class ViewModelFactory  
    {
        private readonly IUnityContainer _container;

        public ViewModelFactory(IUnityContainer container)
        {
            _container = container;
        }

        public TViewModel Create<TViewModel>()
            where TViewModel : IViewModel
        {
            return _container.Resolve<TViewModel>();
        }
    }
}