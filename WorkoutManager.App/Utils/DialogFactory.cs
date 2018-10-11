using System.Windows;
using WorkoutManager.App.Structures;

namespace WorkoutManager.App.Utils
{
    internal interface IDialog<out TDataContext>
    {
        TDataContext Data { get; }

        DialogResult Show();
    }
    
    internal class DialogFactory<TView, TViewModel>
        where TView : Window
        where TViewModel : class, IViewModel
    {
        private readonly WindowFactory _windowFactory;
        private readonly ViewModelFactory<TViewModel> _viewModelFactory;
        
        public DialogFactory(WindowFactory windowFactory, ViewModelFactory<TViewModel> viewModelFactory)
        {
            _windowFactory = windowFactory;
            _viewModelFactory = viewModelFactory;
        }
        
        public IDialog<TViewModel> Get()
        {
            return new Dialog(_windowFactory, _viewModelFactory);
        }

        private class Dialog : IDialog<TViewModel>
        {
            private readonly TView _view;
            
            public TViewModel Data { get; set; }

            public Dialog(WindowFactory windowFactory, ViewModelFactory<TViewModel> viewModelFactory)
            {
                _view = windowFactory.Get<TView>();
                Data = viewModelFactory.Get();
            }

            public DialogResult Show()
            {
                _view.DataContext = Data;
                var dialogResult = _view.ShowDialog();
    
                return dialogResult.HasValue && dialogResult.Value ? DialogResult.Ok : DialogResult.None;
            }
        }
    }
}