using System.Windows;

namespace WorkoutManager.App.Utils
{
    internal enum DialogResult
    {
        None,
        Ok,
    }
    
    internal interface IShowDialog {
        DialogResult Show();
    }
    
    internal class DialogViewer<TDialogWindow> : IShowDialog
        where TDialogWindow : Window, new()
    {
        private object _dataContext;

        public IShowDialog WithContext(object dataContext)
        {
            _dataContext = dataContext;

            return this;
        }
        
        public DialogResult Show()
        {
            var dialog = new TDialogWindow() { DataContext = _dataContext };
            var dialogResult = dialog.ShowDialog();

            return dialogResult.HasValue && dialogResult.Value ? DialogResult.Ok : DialogResult.None;
        }
    }
}