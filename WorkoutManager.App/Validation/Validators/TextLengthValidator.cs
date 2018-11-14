using System.Collections.Generic;

namespace WorkoutManager.App.Validation.Validators
{
    internal class TextLengthValidator
    {
        private readonly double _minimum;
        private readonly double _maximum;
        
        public TextLengthValidator(double minimum = double.MinValue, double maximum = double.MaxValue)
        {
            _minimum = minimum;
            _maximum = maximum;
        }

        public IEnumerable<string> Validate(object item)
        {
            switch (item)
            {
                case null:

                    return new[] { $"Input should have at least {_minimum} characters." };
                case string text:

                    if (string.IsNullOrWhiteSpace(text) || text.Length < _minimum)
                    {
                        return new[] { $"Input should have at least {_minimum} characters." };
                    }

                    if (text.Length > _maximum)
                    {
                        return new[] { $"Input should have at max {_maximum} characters." };
                    }

                    return null;
                default:
                    return new [] { "Input should be a text value." };
            }
        }
    }
}