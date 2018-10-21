using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using static System.Double;

namespace WorkoutManager.App.Validators
{
    internal class DoubleValidationRule : ValidationRule
    {
        public double Minimum { get; set; } = MinValue;

        public double Maximum { get; set; } = MaxValue;
        
        public DoubleValidationRule()
        {
            ValidatesOnTargetUpdated = true;
        }
        
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!(value is string stringValue) || !Regex.IsMatch(stringValue, @"^(0|[1-9][0-9]*)(\.\d+)?$"))
            {
                return new ValidationResult(false, "Enter a valid number");
            }

            var number = Parse(stringValue);
            
            if (number < Minimum)
            {
                return new ValidationResult(false, $"Number is too little, minimum: {Minimum}");
            }
            if (number > Maximum)
            {
                return new ValidationResult(false, $"Number is too big, maximum: {Maximum}");
            }

            return new ValidationResult(true, null);
        }
    }
}