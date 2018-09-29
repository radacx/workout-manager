using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WorkoutManager.App.Controls.TimeSpanUpDown
{
	public class TimeSpanUpDown : TextBox
    {
        #region MaxTime

	    public static readonly DependencyProperty MaxTimeProperty = DependencyProperty.Register(
		    nameof(MaxTime),
		    typeof(string),
		    typeof(TimeSpanUpDown),
		    new PropertyMetadata(default(string), OnMaxTimeChanged)
	    );

	    private static void OnMaxTimeChanged(
			DependencyObject dependencyObject,
			DependencyPropertyChangedEventArgs e)
	    {
		    if (!(dependencyObject is TimeSpanUpDown control))
		    {
			    return;
		    }
		    
		    var timeString = (string) e.NewValue;
		    var timeSpan = ParseTimeSpan(timeString, false);
		    control.MaxTimeSpanChanged(timeSpan);
	    }
	    
	    public string MaxTime
	    {
		    get => (string) GetValue(MaxTimeProperty);
		    set => SetValue(MaxTimeProperty, value);
	    }

		#endregion
		
		#region MinTime

	    public static readonly DependencyProperty MinTimeProperty = DependencyProperty.Register(
		    nameof(MinTime),
		    typeof(string),
		    typeof(TimeSpanUpDown),
		    new PropertyMetadata(default(string), OnMinTimeChanged)
	    );

	    private static void OnMinTimeChanged(
			DependencyObject dependencyObject,
			DependencyPropertyChangedEventArgs e)
	    {
		    if (!(dependencyObject is TimeSpanUpDown control))
		    {
			    return;
		    }
		    
		    var timeString = (string) e.NewValue;
		    var timeSpan = ParseTimeSpan(timeString, false);
		    control.MinTimeSpanChanged(timeSpan);
	    }
	    
	    public string MinTime
	    {
		    get => (string) GetValue(MinTimeProperty);
		    set => SetValue(MinTimeProperty, value);
	    }
	    
		#endregion

		#region Value

	    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
		    nameof(Value),
		    typeof(TimeSpan),
		    typeof(TimeSpanUpDown),
		    new PropertyMetadata(default(TimeSpan), OnValueChanged)
	    );
	    
	    private static void OnValueChanged(
			DependencyObject dependencyObject,
			DependencyPropertyChangedEventArgs e)
	    {
		    if (!(dependencyObject is TimeSpanUpDown control))
		    {
			    return;
		    }
		    
		    var value = (TimeSpan) e.NewValue;

		    if (value > control._maxTimeSpan)
		    {
			    value = control._maxTimeSpan;
		    }

		    if (value < control._minTimeSpan)
		    {
			    value = control._minTimeSpan;
		    }

		    control.Value = value;
		    control.Text = GetFormattedTime(value, control._formatString);
	    }
	    
	    public TimeSpan Value
	    {
		    get => (TimeSpan) GetValue(ValueProperty);
		    set => SetValue(ValueProperty, value);
	    }

		#endregion

		#region TimeFormat

	    public static readonly DependencyProperty TimeFormatProperty = DependencyProperty.Register(
		    nameof(TimeFormat),
		    typeof(TimeFormat),
		    typeof(TimeSpanUpDown),
		    new PropertyMetadata(default(TimeFormat), OnTimeFormatChanged)
	    );

	    private static void OnTimeFormatChanged(
			DependencyObject dependencyObject,
			DependencyPropertyChangedEventArgs e)
	    {
		    if (!(dependencyObject is TimeSpanUpDown control))
		    {
			    return;
		    }
		    
		    control.TimeFormatChanged((TimeFormat) e.NewValue);
	    }
	    
	    public TimeFormat TimeFormat
	    {
		    get => (TimeFormat) GetValue(TimeFormatProperty);
		    set => SetValue(TimeFormatProperty, value);
	    }

		#endregion


	    private void MaxTimeSpanChanged(TimeSpan timeSpan)
	    {
		    _maxTimeSpan = timeSpan;
		    
		    if (timeSpan.TotalSeconds >= (100 * 60 * 60))
		    {
			    _formatString = "hhh:mm:ss";
			    _maxCharacterValues = timeSpan.Hours.ToString().Substring(0, 1) + "99:59:59.999";
		    }

		    if (timeSpan.TotalSeconds >= (10 * 60 * 60))
		    {
			    _formatString = "hh:mm:ss";
			    _maxCharacterValues = timeSpan.Hours.ToString().Substring(0, 1) + "9:59:59.999";
		    }
		    else if (timeSpan.TotalSeconds >= (60 * 60))
		    {
			    _formatString = "h:mm:ss";
			    _maxCharacterValues = timeSpan.Hours.ToString() + ":59:59.999";
		    }
		    else if (timeSpan.TotalSeconds >= (10 * 60))
		    {
			    _formatString = "mm:ss";
			    _maxCharacterValues = timeSpan.Minutes.ToString().Substring(1) + "9:59.999";
		    }
		    else if (timeSpan.TotalSeconds >= (60))
		    {
			    _formatString = "m:ss";
			    _maxCharacterValues = timeSpan.Minutes.ToString() + ":59.999";
		    }
		    else if (timeSpan.TotalSeconds >= (10))
		    {
			    _formatString = "ss";
			    _maxCharacterValues = timeSpan.Seconds.ToString().Substring(1) + "9.999";
		    }
		    else
		    {
			    _formatString = "s";
			    _maxCharacterValues = timeSpan.Seconds.ToString() + ".999";
		    }


		    switch (TimeFormat)
		    {
			    case TimeFormat.Seconds10Ths:
				    _formatString += ".f   ";
				    _millisecondAdjustment = 100;

				    break;
			    case TimeFormat.Seconds100Ths:
				    _formatString += ".ff  ";
				    _millisecondAdjustment = 10;

				    break;
			    case TimeFormat.Seconds1000Ths:
				    _formatString += ".fff ";
				    _millisecondAdjustment = 1;

				    break;
			    case TimeFormat.Minutes:
				    _formatString = _formatString.Substring(0, _formatString.Length - 3) + " ";

				    break;
		    }

		    Text = GetFormattedTime(Value, _formatString);
	    }

	    private void MinTimeSpanChanged(TimeSpan timeSpan)
	    {
		    _minTimeSpan = timeSpan;

		    Text = GetFormattedTime(Value, _formatString);
	    }

	    private void TimeFormatChanged(TimeFormat timeFormat) => MaxTimeSpanChanged(_maxTimeSpan);

	    // ff hundredths of second
	    private const string DefaultMask = "hh:mm:ss.ff";

	    private TimeSpan _maxTimeSpan;
	    private TimeSpan _minTimeSpan;
	    private string _maxCharacterValues = string.Empty;
	    private double _millisecondAdjustment = 100;
	    private string _formatString;
	    
	    public TimeSpanUpDown()
	    {
		    _formatString = DefaultMask;
	    }

	    private void ReplaceDigit(int position, char digit, bool normal)
	    {
		    if (position >= Text.Length || position < 0)
		    {
			    return;
		    }

		    string firstPart, lastPart;

		    if (!":.".Contains(Text[position]))
		    {
			    firstPart = Text.Substring(0, position);
			    lastPart = Text.Substring(position + 1);
		    }
		    else if (normal)
		    {
			    firstPart = Text.Substring(0, position + 1);
			    lastPart = Text.Substring(position + 2);
		    }
		    else
		    {
			    firstPart = Text.Substring(0, position - 1);
			    lastPart = Text.Substring(position);
			    position--;
		    }

		    var isMinutesFormat = TimeFormat == TimeFormat.Minutes;
		    firstPart = firstPart + digit + lastPart;
		    var value = ParseTimeSpan(firstPart, isMinutesFormat);

		    if (value > _maxTimeSpan)
		    {
			    firstPart = GetFormattedTime(_maxTimeSpan, _formatString);
		    }

		    if (value < _minTimeSpan)
		    {
			    firstPart = GetFormattedTime(_minTimeSpan, _formatString);
		    }

		    Text = firstPart;
		    Value = ParseTimeSpan(firstPart, isMinutesFormat);
		    
		    SelectionStart = normal ? position + 1 : position;
	    }
	    
	    private void ChangeValueDependingOnLocation(int amount)
	    {
		    var selectionStart = SelectionStart;

		    if (_formatString.Length < selectionStart + 1)
		    {
			    return;
		    }
			
		    // this should work for milliseconds but was bugged
		    /* var increment = (_formatString[selectionStart] != _formatString[selectionStart + 1]
				    ? 1
				    : (_formatString[selectionStart] != _formatString[selectionStart + 2])
					    ? 10
					    : 100)
			    * amount; */

		    // this works when we use seconds as lowest unit
		    var increment = _formatString.Length == selectionStart + 1
			    ? 1
			    : _formatString[selectionStart] != _formatString[selectionStart + 1]
				    ? 1
				    : 10;
			
		    increment *= amount;
				
		    var currentTimeSpan = ParseTimeSpan(Text, TimeFormat == TimeFormat.Minutes);

		    switch (_formatString[selectionStart])
		    {
			    case 'm':
				    currentTimeSpan = currentTimeSpan.Add(TimeSpan.FromMinutes(increment));

				    break;
			    case 'h':
				    currentTimeSpan = currentTimeSpan.Add(TimeSpan.FromHours(increment));

				    break;
			    case 's':
				    currentTimeSpan = currentTimeSpan.Add(TimeSpan.FromSeconds(increment));

				    break;
			    case 'f':
				    currentTimeSpan = currentTimeSpan.Add(TimeSpan.FromMilliseconds(increment * _millisecondAdjustment));

				    break;
		    }

		    if (currentTimeSpan > _maxTimeSpan)
		    {
			    currentTimeSpan = _maxTimeSpan;
		    }

		    if (currentTimeSpan < _minTimeSpan)
		    {
			    currentTimeSpan = _minTimeSpan;
		    }

		    Value = currentTimeSpan;
		    Text = GetFormattedTime(currentTimeSpan, _formatString);
		    SelectionStart = selectionStart;
	    }
	    
	    private bool TestCaretAtEnd(char value, int position) => value <= _maxCharacterValues[position];

	    protected override void OnInitialized(EventArgs e)
	    {
		    if (Value < _minTimeSpan)
		    {
			    Value = _minTimeSpan;
		    } else if (Value > _maxTimeSpan)
		    {
			    Value = _maxTimeSpan;
		    }
		    
		    base.OnInitialized(e);
	    }

	    protected override void OnPreviewTextInput(TextCompositionEventArgs e)
	    {
		    base.OnPreviewTextInput(e);
		    
		    if (char.IsDigit(e.Text[0]))
		    {
			    if (e.Text.Any(c => TestCaretAtEnd(c, SelectionStart)))
			    {
				    ReplaceDigit(SelectionStart, e.Text[0], true);
			    }
			    else if (Text.Substring(0, SelectionStart - 1).Replace(':', '0').All(i => i == '0'))
			    {
				    ChangeValueDependingOnLocation(e.Text[0] - Text[SelectionStart]);
			    }
		    }

		    e.Handled = true;
	    }

	    protected override void OnPreviewKeyDown(KeyEventArgs e)
	    {
		    base.OnPreviewKeyDown(e);
		    
		    switch (e.Key) {
			    case Key.Space:
				    e.Handled = true;

				    break;
			    case Key.Back:
				    ReplaceDigit(SelectionStart - 1, '0', false);
				    e.Handled = true;

				    break;
			    case Key.Delete:
				    ReplaceDigit(SelectionStart, '0', true);
				    e.Handled = true;

				    break;
			    case Key.Right: {
				    if (Text.Length > SelectionStart - 1)
				    {
					    SelectionStart++;
				    }

				    e.Handled = true;

				    break;
			    }
			    case Key.Left: {
				    var caret = SelectionStart - 1;

				    if (caret < 0)
				    {
					    return;
				    }

				    SelectionStart = (":.".Contains(Text[caret]))
					    ? SelectionStart - 2
					    : SelectionStart - 1;

				    e.Handled = true;

				    break;
			    }
			    case Key.Up:
				    ChangeValueDependingOnLocation(1);
				    e.Handled = true;

				    break;
			    case Key.Down:
				    ChangeValueDependingOnLocation(-1);
				    e.Handled = true;

				    break;
		    }
	    }

	    protected override void OnSelectionChanged(RoutedEventArgs e)
	    {
		    base.OnSelectionChanged(e);
		    
		    if (Text.Length > SelectionStart)
		    {
			    if (":.".Contains(Text[SelectionStart]))
				    SelectionStart++;

			    if (SelectionLength != 1 && SelectionStart < Text.Length)
				    SelectionLength = 1;
		    }
	    }

	    private static string GetFormattedTime(TimeSpan timeSpan, string formatString)
		{
			var hours = Math.Floor(timeSpan.TotalHours).ToString("000");
			var minutes = timeSpan.Minutes.ToString("00");
			var seconds = timeSpan.Seconds.ToString("00");
			var milliseconds = timeSpan.Milliseconds.ToString("000");
			var newString = formatString;

			if (formatString.Contains("hhh"))
			{
				newString = newString.Replace("hhh", hours);
			}

			if (formatString.Contains("hh"))
			{
				newString = newString.Replace("hh", hours.Substring(1));
			}
			else if (formatString.Contains("h"))
			{
				newString = newString.Replace('h', hours[2]);
			}

			if (formatString.Contains("mm"))
			{
				newString = newString.Replace("mm", minutes);
			}
			else if (formatString.Contains("m"))
			{
				newString = newString.Replace("m", minutes.Substring(1));
			}

			if (formatString.Contains("ss"))
			{
				newString = newString.Replace("ss", seconds);
			}
			else if (formatString.Contains("s"))
			{
				newString = newString.Replace("s", seconds.Substring(1));
			}

			if (formatString.Contains("fff"))
			{
				newString = newString.Replace("fff", milliseconds);
			}
			else if (formatString.Contains("ff"))
			{
				newString 	= newString.Replace("ff", milliseconds.Substring(0, 2));
			}
			else if (formatString.Contains("f"))
			{
				newString = newString.Replace("f", milliseconds.Substring(0, 1));
			}

			return newString.Trim();
		}

		private static TimeSpan ParseTimeSpan(string value, bool minutesFormat)
		{
			if (minutesFormat)
			{
				value += ":00";
			}

			try
			{
				int hours = 0, minutes = 0;
				var timePieces = value.Split(':');
				var secondsWithFraction = double.Parse(timePieces.Last());
				var seconds = Math.Floor(secondsWithFraction);
				var milliseconds = (int) ((secondsWithFraction - seconds) * 1000);

				if (timePieces.Length > 1)
				{
					minutes = int.Parse(timePieces[timePieces.Length - 2]);
				}

				if (timePieces.Length > 2)
				{
					hours = int.Parse(timePieces[timePieces.Length - 3]);
				}

				return new TimeSpan(
					0,
					hours,
					minutes,
					(int) seconds,
					milliseconds
				);
			}
			catch
			{
				throw new InvalidCastException($"The value '{value}' cannot be converted to a TimeSpan");
			}
		}
    }
}
