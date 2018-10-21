using System.Globalization;
using System.Windows.Controls;

namespace WorkoutManager.App.Validators
{
    internal class RequiredTextValidationRule : ValidationRule
    {
        public string ErrorMessage { get; set; } = "Provide text";
        
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string stringValue && !string.IsNullOrWhiteSpace(stringValue))
            {
                return new ValidationResult(true, null);
            }
            
            return new ValidationResult(false, ErrorMessage);
        }

        public RequiredTextValidationRule()
        {
            ValidatesOnTargetUpdated = true;
        }
    }
}