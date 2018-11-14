using System.Collections.Generic;
using System.Text.RegularExpressions;
using static System.Double;

namespace WorkoutManager.App.Validation.Validators
{
    internal class DoubleValidator
    {
        private readonly double _minimum;
        private readonly double _maximum;
        
        public DoubleValidator(double minimum = MinValue, double maximum = MaxValue)
        {
            _minimum = minimum;
            _maximum = maximum;
        }
        
        public IEnumerable<string> Validate(string stringValue)
        {
            if (!(Regex.IsMatch(stringValue, @"^-?[1-9]\d*(\.\d+)?$|^0(\.\d+)?$")
                && TryParse(stringValue, out var number)))
            {
                return new[] { "Input should be a number." };
            }

            if (number < _minimum)
            {
                return new [] { $"Input should be a number greater or equal than {_minimum}." }; 
            }
            
            if (number > _maximum)
            {
                return new [] { $"Input should be a number lesser or equal than {_maximum}." }; 
            }
            
            return null;
        }
    }
}