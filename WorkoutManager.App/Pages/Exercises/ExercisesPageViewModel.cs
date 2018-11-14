using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Input;
using PubSub.Core;
using WorkoutManager.App.Events;
using WorkoutManager.App.Pages.Exercises.Dialogs.ExerciseDialog;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Structures.Collections.Common;
using WorkoutManager.App.Structures.Dialogs;
using WorkoutManager.App.Structures.ViewModels;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Pages.Exercises
{
    internal class ExercisesPageViewModel : ViewModelBase
    {
        public static string DialogsIdentifier => "ExercisesPageDialogs";

        private readonly Repository<Exercise> _exerciseRepository;
        private readonly DialogFactory _dialogs;
        private readonly Hub _hub;
        
        
        #region Commands
        
        public ICommand OpenCreateExerciseDialogCommand { get; }
        
        public ICommand OpenEditExerciseDialogCommand { get; }
        
        public ICommand DeleteCommand { get; }
        
        #endregion

        
        #region UI Properties

        public ObservableRangeCollection<Exercise> Exercises { get; } = new WpfObservableRangeCollection<Exercise>();

        #endregion
        

        #region ExerciseDialog

        private async void OpenCreateExerciseDialogAsync()
        {
            var exercise = new Exercise
            {
                ContractionType = ContractionType.Dynamic,
            };

            var dialog = _dialogs.For<ExerciseDialogViewModel>(DialogsIdentifier);
            dialog.Data.DialogTitle = "New exercise";
            dialog.Data.SubmitButtonTitle = "Create";
            dialog.Data.Exercise = ExerciseViewModel.FromModel(exercise);

            var dialogResult = await dialog.Show();

            if (dialogResult != DialogResult.Ok)
            {
                return;
            }

            exercise = dialog.Data.Exercise.ToModel();
            Exercises.Add(exercise);
            _exerciseRepository.Create(exercise);
        }

        private async void OpenEditExerciseDialogAsync(Exercise exercise)
        {
            var exerciseClone = exercise.Clone();

            var dialog = _dialogs.For<ExerciseDialogViewModel>(DialogsIdentifier);
            dialog.Data.Exercise = ExerciseViewModel.FromModel(exerciseClone);
            dialog.Data.SubmitButtonTitle = "Save";
            dialog.Data.DialogTitle = "Modified exercise";
                    
            var dialogResult = await dialog.Show();

            if (dialogResult != DialogResult.Ok)
            {
                return;
            }

            exerciseClone = dialog.Data.Exercise.ToModel();
            Exercises.Replace(exercise, exerciseClone);
            _exerciseRepository.Update(exerciseClone);
        }
        
        #endregion
        
               
        private void LoadExercises() => Exercises.AddRange(_exerciseRepository.GetAll());

        private void DeleteExercise(Exercise exercise)
        {
            Exercises.Remove(exercise);

            Task.Run(() => _exerciseRepository.Delete(exercise));
        }
        
        public ExercisesPageViewModel(Repository<Exercise> exerciseRepository, DialogFactory dialogs, Hub hub)
        {
            _exerciseRepository = exerciseRepository;
            _dialogs = dialogs;
            _hub = hub;

            OpenCreateExerciseDialogCommand = new Command(OpenCreateExerciseDialogAsync);
            OpenEditExerciseDialogCommand = new Command<Exercise>(OpenEditExerciseDialogAsync);
            DeleteCommand = new Command<Exercise>(DeleteExercise);
            
            Exercises.CollectionChanged += ExercisesOnCollectionChanged;
            
            Task.Run(LoadExercises);
        }

        private void ExercisesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _hub.Publish(new ExercisesChangedEvent(Exercises));
        }
    }
}
