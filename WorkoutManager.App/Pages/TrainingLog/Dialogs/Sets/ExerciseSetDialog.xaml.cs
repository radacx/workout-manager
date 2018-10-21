using System.Windows;
using System.Windows.Controls;

namespace WorkoutManager.App.Pages.TrainingLog.Dialogs
{
    internal partial class ExerciseSetDialog : UserControl
    {
        public new static readonly DependencyProperty DataContextProperty = DependencyProperty.Register(
            "DataContext",
            typeof(object),
            typeof(ExerciseSetDialog),
            new PropertyMetadata(default(object))
        );

        public new object DataContext
        {
            get => (object) GetValue(DataContextProperty);
            set => SetValue(DataContextProperty, value);
        }
        
        public ExerciseSetDialog()
        {
            InitializeComponent();
        }
    }
}
