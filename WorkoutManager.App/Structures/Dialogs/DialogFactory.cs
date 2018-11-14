using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using WorkoutManager.App.Structures.ViewModels;

namespace WorkoutManager.App.Structures.Dialogs
{
    internal class DialogFactory
    {
        private readonly ViewModelFactory _viewModelFactory;
        
        public DialogFactory(ViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }
        
        public IDialog<TViewModel> For<TViewModel>(string identifier = null)
            where TViewModel : class, IViewModel
        {
            return new Dialog<TViewModel>(_viewModelFactory, identifier);
        }  

        private class Dialog<TViewModel> : IDialog<TViewModel>
            where TViewModel : class, IViewModel
        {
            private readonly string _identifier;
            
            public TViewModel Data { get; }

            public Dialog(ViewModelFactory viewModelFactory, string identifier)
            {
                _identifier = identifier;
                Data = viewModelFactory.Create<TViewModel>();
            }

            public async Task<DialogResult> Show()
            {
                var result = await DialogHost.Show(Data, _identifier);
                
                if (result is DialogResult dialogResult)
                {
                    return dialogResult;
                }

                return DialogResult.None;
            }
        }
    }
}