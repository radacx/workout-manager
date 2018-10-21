using System;
using System.Windows.Input;

namespace WorkoutManager.App.Structures
{
    internal class Command : ICommand
    {
        private readonly Action _action;
        private readonly Func<bool> _canExecute;

        public Command(Action action, Func<bool> canExecute = null)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        public void Execute(object parameter) => _action.Invoke();

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        
        public event EventHandler CanExecuteChanged;
    }
    
    internal class Command<TParameter> : ICommand
    {
        private readonly Action<TParameter> _action;
        private readonly Func<TParameter, bool> _canExecute;

        public Command(Action<TParameter> action, Func<TParameter, bool> canExecute = null)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (parameter == null)
            {
                return true;
            }
            
            if (!(parameter is TParameter typedParameter))
            {
                throw new ArgumentException("Invalid parameter type");
            }

            return _canExecute == null
                || _canExecute(typedParameter);
        }

        public void Execute(object parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }
            
            if (!(parameter is TParameter typedParameter))
            {
                throw new ArgumentException($"Invalid parameter type, Expected: {typeof(TParameter).Name}, Actual: {parameter.GetType().Name}");
            }

            _action.Invoke(typedParameter);
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        
        public event EventHandler CanExecuteChanged;
    }
}