using WorkoutManager.App.Structures.ViewModels;
using WorkoutManager.App.Validation.Validators;

namespace WorkoutManager.App.Pages.Progress.Dialogs
{
    internal class ProgressFilterViewModel : ViewModelBase
    {
        private bool _rememberFilterBy;
        private bool _rememberGroupBy;
        private bool _rememberMetric;
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                var validator = new TextLengthValidator(minimum: 0);
                var validationResults = validator.Validate(value);
                SetValidationResults(validationResults);
                
                if (!SetField(ref _name, value) || validationResults != null) {}
            }
        }

        public bool RememberFilterBy
        {
            get => _rememberFilterBy;
            set
            {
                if (SetField(ref _rememberFilterBy, value))
                {
                    ValidateBoolCombinations();
                }
            }
        }

        public bool RememberGroupBy
        {
            get => _rememberGroupBy;
            set
            {
                if (SetField(ref _rememberGroupBy, value))
                {
                    ValidateBoolCombinations();
                }
            }
        }

        public bool RememberMetric
        {
            get => _rememberMetric;
            set
            {
                if (SetField(ref _rememberMetric, value))
                {
                    ValidateBoolCombinations();
                }
            }
        }

        private void ValidateBoolCombinations()
        {
            var isValid = RememberFilterBy || RememberGroupBy || RememberMetric;
            var validationResults = isValid ? null : new[] { "Select at least one filter option" };
            SetValidationResults(validationResults);
        }
    }
}