using Unity;

namespace WorkoutManager.App.Structures.ViewModels
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