using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using WorkoutManager.Contract.Structures;

namespace WorkoutManager.App.Structures.ViewModels
{
    internal abstract class ViewModelBase : PropertyChangedNotifier, INotifyDataErrorInfo, IViewModel
    {
        private readonly IDictionary<string, IEnumerable<string>> _errors = new Dictionary<string, IEnumerable<string>>();

        public IEnumerable GetErrors(string propertyName)
            => _errors.TryGetValue(propertyName, out var errors) ? errors : null;

        public bool HasErrors => _errors.Any();
        
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected void SetValidationResults(IEnumerable<string> validationResults, [CallerMemberName] string propertyName = "")
        {
            if (validationResults == null)
            {
                if (!_errors.ContainsKey(propertyName))
                {
                    return;
                }

                _errors.Remove(propertyName);
            }
            else
            {
                _errors[propertyName] = validationResults;
            }
            
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(nameof(HasErrors));
        }
    }
}