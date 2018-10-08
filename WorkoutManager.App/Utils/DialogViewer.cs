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

        IShowDialog WithContext(object dataContext);
    }
    
    internal static class DialogBuilder
    {
        private class DialogViewer<TDialogWindow> : IShowDialog
            where TDialogWindow : Window, new()
        {
            private object _dataContext;

            public DialogResult Show()
            {
                var dialog = new TDialogWindow() { DataContext = _dataContext };
                var dialogResult = dialog.ShowDialog();

                return dialogResult.HasValue && dialogResult.Value ? DialogResult.Ok : DialogResult.None;
            }
            
            public IShowDialog WithContext(object dataContext)
            {
                var dialogViewer = new DialogViewer<TDialogWindow> { _dataContext = dataContext };

                return dialogViewer;
            }
        }

        public static IShowDialog Create<TDialogWindow>()
            where TDialogWindow : Window, new()
        {
            return new DialogViewer<TDialogWindow>();
        }
    }
}