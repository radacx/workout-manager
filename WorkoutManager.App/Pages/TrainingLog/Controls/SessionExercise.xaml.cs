using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WorkoutManager.App.Pages.TrainingLog.Controls
{
    public partial class SessionExercise : UserControl
    {
        public static readonly DependencyProperty OnAddExerciseProperty = DependencyProperty.Register(
            nameof(OnAddExercise),
            typeof(ICommand),
            typeof(SessionExercise),
            new PropertyMetadata(default(ICommand))
        );

        public ICommand OnAddExercise
        {
            get => (ICommand) GetValue(OnAddExerciseProperty);
            set => SetValue(OnAddExerciseProperty, value);
        }

        public static readonly DependencyProperty OnDeleteSetProperty = DependencyProperty.Register(
            nameof(OnDeleteSet),
            typeof(ICommand),
            typeof(SessionExercise),
            new PropertyMetadata(default(ICommand))
        );
        
        public ICommand OnDeleteSet
        {
            get => (ICommand) GetValue(OnDeleteSetProperty);
            set => SetValue(OnDeleteSetProperty, value);
        }

        public static readonly DependencyProperty OnDeleteExerciseProperty = DependencyProperty.Register(
            nameof(OnDeleteExercise),
            typeof(ICommand),
            typeof(SessionExercise),
            new PropertyMetadata(default(ICommand))
        );

        public ICommand OnDeleteExercise
        {
            get => (ICommand) GetValue(OnDeleteExerciseProperty);
            set => SetValue(OnDeleteExerciseProperty, value);
        }
          
        public SessionExercise()
        {
            InitializeComponent();
        }
    }
}
