using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Force.DeepCloner;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils;
using WorkoutManager.App.Utils.Dialogs;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Pages.Exercises.Models
{
    internal class ExercisedMuscleDialogSelector : DataTemplateSelector
    {
        public const string AddExercisedMuscleDataTemplateIdentifier = "AddExercisedMuscleDataTemplateIdentifier";
        public const string EditExercisedMuscleDataTemplateIdentifier = "EditExercisedMuscleDataTemplateIdentifier";
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item) {
                case AddExercisedMuscleDialogViewModel _:

                    return Application.Current.MainWindow?.FindResource(AddExercisedMuscleDataTemplateIdentifier) as DataTemplate;
                case EditExercisedMuscleDialogViewModel _:

                    return Application.Current.MainWindow?.FindResource(EditExercisedMuscleDataTemplateIdentifier) as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    } 
    
    internal class ExerciseDialogModelViewModel : DialogModelBase
    {
        private readonly DialogViewer _dialogViewer;
        
        private Exercise _exercise;

        public string ExercisedMuscleDialogIdentifier => "ExercisedMuscleDialog";

        public Exercise Exercise
        {
            get => _exercise;
            set => SetField(ref _exercise, value);
        }

        public ObservableRangeCollection<Muscle> Muscles { get; } = new WpfObservableRangeCollection<Muscle>();
        
        public ICommand RemoveMuscle { get; }
        
        public Command OpenAddMuscleDialogCommand { get; }

        public ICommand OpenEditMuscleDialog { get; }
        
        public ObservedCollection<ExercisedMuscle> ExercisedMuscles { get; set; }

        private bool IsMuscleAvailable(Muscle muscle)
            => !ExercisedMuscles.Any(exercisedMuscle => exercisedMuscle.Muscle.Equals(muscle));

        private async void OpenAddMuscleDialog()
        {
            var dialog = _dialogViewer.For<AddExercisedMuscleDialogViewModel>(ExercisedMuscleDialogIdentifier);
            dialog.Data.Muscles = Muscles;
            dialog.Data.SubmitButtonTitle = "Select";
            dialog.Data.DialogTitle = "Add a muscle";
            dialog.Data.RelativeEngagement = 100;

            var dialogResult = await dialog.Show();

            if (dialogResult != DialogResult.Ok)
            {
                return;
            }

            var muscle = dialog.Data.SelectedMuscle;
            var relativeEngagement = dialog.Data.RelativeEngagement;
            var exercisedMuscle = new ExercisedMuscle(muscle, relativeEngagement);

            ExercisedMuscles.Add(exercisedMuscle);
            OpenAddMuscleDialogCommand.RaiseCanExecuteChanged();
        }
        
        public ExerciseDialogModelViewModel(Repository<Muscle> muscleRepository, DialogViewer dialogViewer)
        {
            _dialogViewer = dialogViewer;

            var availableMusclesShape = Muscles.ShapeView()
                .Where(IsMuscleAvailable)
                .OrderBy(muscle => muscle.Name);
            
            availableMusclesShape.Apply();

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Exercise))
                {
                    var exercisedMuscles = new ObservedCollection<ExercisedMuscle>(
                        new List<ExercisedMuscle>(), 
                        muscle =>
                        {
                            Exercise.AddMuscle(muscle);
                            availableMusclesShape.Apply();
                        },
                        muscle =>
                        {
                            Exercise.RemoveMuscle(muscle);
                            availableMusclesShape.Apply();
                        }
                    );
                    
                    exercisedMuscles.AddRange(Exercise.Muscles);       
                    exercisedMuscles.ShapeView().OrderBy(muscle => muscle.Muscle.Name).Apply();

                    ExercisedMuscles = exercisedMuscles;
                }
            };

            OpenAddMuscleDialogCommand = new Command(
                OpenAddMuscleDialog,
                () => Muscles.Where(IsMuscleAvailable).Any()
            );
            
            OpenEditMuscleDialog = new Command<ExercisedMuscle>(
                async exercisedMuscle =>
                {
                    var exercisedMuscleClone = exercisedMuscle.DeepClone();
                    var dialog = dialogViewer.For<EditExercisedMuscleDialogViewModel>(ExercisedMuscleDialogIdentifier);
  
                    dialog.Data.SubmitButtonTitle = "Save";
                    dialog.Data.DialogTitle = "Edit exercised muscle";
                    dialog.Data.ExercisedMuscle = exercisedMuscleClone;
                    
                    var dialogResult = await dialog.Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    ExercisedMuscles.Replace(exercisedMuscle, exercisedMuscleClone);
                });
            
            RemoveMuscle = new Command<ExercisedMuscle>(muscle =>
                {
                    ExercisedMuscles.Remove(muscle);
                    OpenAddMuscleDialogCommand.RaiseCanExecuteChanged();
                }
            );

            Task.Run(
                () => { Muscles.AddRange(muscleRepository.GetAll()); }
            );
        }
    }
}