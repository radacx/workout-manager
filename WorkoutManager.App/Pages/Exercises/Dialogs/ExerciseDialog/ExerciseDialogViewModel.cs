using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutManager.App.Pages.Exercises.Dialogs.AddMuscleDialog;
using WorkoutManager.App.Pages.Exercises.Dialogs.EditMuscleDialog;
using WorkoutManager.App.Pages.Exercises.Dialogs.Models;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Structures.Dialogs;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Pages.Exercises.Dialogs.ExerciseDialog
{
    internal class ExerciseDialogViewModel : DialogModelBase
    {
        public static string DialogsIdentifier => "ExerciseDialogDialogs";

        private readonly Repository<Muscle> _muscleRepository;
        private readonly DialogFactory _dialogs;
        
        private readonly List<Muscle> _availableMuscles = new List<Muscle>();

        
        #region Commands

        public ICommand RemoveMuscleCommand { get; }
        
        public Command OpenAddMuscleDialogCommand { get; }

        public ICommand OpenEditMuscleDialogCommand { get; }

        #endregion


        #region UI Properties

        public ExerciseViewModel Exercise { get; set; }

        #endregion
        
        
        #region AddExercisedMuscleDialog

        private async void OpenAddMuscleDialogAsync()
        {
            var exercisedMuscle = new ExercisedMuscle
            {
                RelativeEngagement = 100d
            };
            
            var dialog = _dialogs.For<AddExercisedMuscleDialogViewModel>(DialogsIdentifier);
            dialog.Data.DialogTitle = "Add a muscle";
            dialog.Data.SubmitButtonTitle = "Select";
            dialog.Data.AvailableMuscles = _availableMuscles.Where(IsMuscleAvailable);
            dialog.Data.ExercisedMuscle = ExercisedMuscleViewModel.FromModel(exercisedMuscle);

            var dialogResult = await dialog.Show();

            if (dialogResult != DialogResult.Ok)
            {
                return;
            }

            exercisedMuscle = dialog.Data.ExercisedMuscle.ToModel();

            Exercise.Muscles.Add(exercisedMuscle);
            Exercise.Muscles.Refresh(exercisedMuscle);
            OpenAddMuscleDialogCommand.RaiseCanExecuteChanged();
        }

        private async void OpenEditMuscleDialogAsync(ExercisedMuscle exercisedMuscle)
        {
            var exercisedMuscleClone = exercisedMuscle.Clone();
            
            var dialog = _dialogs.For<EditExercisedMuscleDialogViewModel>(DialogsIdentifier); 
            dialog.Data.DialogTitle = "Edit exercised muscle";
            dialog.Data.SubmitButtonTitle = "Save";
            dialog.Data.ExercisedMuscle = ExercisedMuscleViewModel.FromModel(exercisedMuscleClone);
                    
            var dialogResult = await dialog.Show();

            if (dialogResult != DialogResult.Ok)
            {
                return;
            }

            exercisedMuscleClone = dialog.Data.ExercisedMuscle.ToModel();
            Exercise.Muscles.Replace(exercisedMuscle, exercisedMuscleClone);
        }
        
        #endregion


        private bool IsMuscleAvailable(Muscle muscle)
            => !Exercise.Muscles.Any(exercisedMuscle => exercisedMuscle.Muscle.Equals(muscle));

        private void LoadData()
        {
            _availableMuscles.AddRange(_muscleRepository.GetAll().OrderBy(x => x.Name));
        }
        
        private void DeleteMuscle(ExercisedMuscle muscle)
        {
            Exercise.Muscles.RemoveByReference(muscle);
            
            OpenAddMuscleDialogCommand.RaiseCanExecuteChanged();
        }
        
        public ExerciseDialogViewModel(Repository<Muscle> muscleRepository, DialogFactory dialogs)
        {
            _muscleRepository = muscleRepository;
            _dialogs = dialogs;

            OpenAddMuscleDialogCommand = new Command(
                OpenAddMuscleDialogAsync,
                () => _availableMuscles.Where(IsMuscleAvailable).Any()
            );
            
            OpenEditMuscleDialogCommand = new Command<ExercisedMuscle>(OpenEditMuscleDialogAsync);
            RemoveMuscleCommand = new Command<ExercisedMuscle>(DeleteMuscle);

            Task.Run(LoadData);
        }
    }
}