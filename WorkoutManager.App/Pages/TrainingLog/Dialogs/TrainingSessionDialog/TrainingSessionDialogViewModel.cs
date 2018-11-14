using System.Windows.Input;
using WorkoutManager.App.Pages.TrainingLog.Dialogs.SessionExerciseDialog;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Structures.Dialogs;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Sessions;

namespace WorkoutManager.App.Pages.TrainingLog.Dialogs.TrainingSessionDialog
{
    internal class TrainingSessionDialogViewModel : DialogModelBase
    {
        public static string DialogsIdentifier => "TrainingSessionDialogDialogs";

        private readonly DialogFactory _dialogs;


        #region Commands

        public ICommand OpenAddSessionExerciseCommand { get; } 
        
        public ICommand OpenEditSessionExerciseCommand { get; } 
        
        public ICommand RemoveExerciseCommand { get; }

        #endregion


        #region UI Properties

        public TrainingSessionViewModel TrainingSession { get; set; }
        
        #endregion


        #region SessionExerciseDialog

        private async void OpenAddSessionExerciseDialogAsync()
        {
            var exercise = new SessionExercise();
            
            var dialog = _dialogs.For<SessionExerciseDialogViewModel>(DialogsIdentifier);
            dialog.Data.DialogTitle = "New session exercise";
            dialog.Data.SubmitButtonTitle = "Create";
            dialog.Data.SessionExercise = SessionExerciseViewModel.FromModel(exercise);

            var dialogResult = await dialog.Show();

            if (dialogResult != DialogResult.Ok)
            {
                return;
            }

            exercise = dialog.Data.SessionExercise.ToModel();
            TrainingSession.Exercises.Add(exercise);
        }      
        
        private async void OpenEditSessionExerciseDialogAsync(SessionExercise sessionExercise)
        {
            var sessionExerciseClone = sessionExercise.Clone();
            
            var dialog = _dialogs.For<SessionExerciseDialogViewModel>(DialogsIdentifier);
            dialog.Data.DialogTitle = "Modified session exercise";
            dialog.Data.SubmitButtonTitle = "Save";
            dialog.Data.SessionExercise = SessionExerciseViewModel.FromModel(sessionExerciseClone);

            var dialogResult = await dialog.Show();

            if (dialogResult != DialogResult.Ok)
            {
                return;
            }

            sessionExerciseClone = dialog.Data.SessionExercise.ToModel();
            TrainingSession.Exercises.Replace(sessionExercise, sessionExerciseClone);
        }      

        #endregion
        
        private void DeleteExercise(SessionExercise exercise)
        {
            TrainingSession.Exercises.RemoveByReference(exercise);
        }
        
        public TrainingSessionDialogViewModel(DialogFactory dialogs)
        {
            _dialogs = dialogs;
            
            OpenAddSessionExerciseCommand = new Command(OpenAddSessionExerciseDialogAsync);
            OpenEditSessionExerciseCommand = new Command<SessionExercise>(OpenEditSessionExerciseDialogAsync);
            RemoveExerciseCommand = new Command<SessionExercise>(DeleteExercise);
        }
    }
}