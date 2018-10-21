using System.Windows;
using System.Windows.Controls;

namespace WorkoutManager.App.Controls
{
    internal partial class ValueSlider : UserControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(double),
            typeof(ValueSlider),
            new PropertyMetadata(default(double))
        );

        public double Value
        {
            get => (double) GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum",
            typeof(double),
            typeof(ValueSlider),
            new PropertyMetadata(default(double))
        );

        public double Minimum
        {
            get => (double) GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum",
            typeof(double),
            typeof(ValueSlider),
            new PropertyMetadata(default(double))
        );

        public double Maximum
        {
            get => (double) GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public static readonly DependencyProperty TickFrequencyProperty = DependencyProperty.Register(
            "TickFrequency",
            typeof(double),
            typeof(ValueSlider),
            new PropertyMetadata(default(double))
        );

        public double TickFrequency
        {
            get => (double) GetValue(TickFrequencyProperty);
            set => SetValue(TickFrequencyProperty, value);
        }
        
        public ValueSlider()
        {
            InitializeComponent();
        }
    }
}
