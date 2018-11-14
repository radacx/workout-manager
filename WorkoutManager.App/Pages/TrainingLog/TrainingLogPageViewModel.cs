using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using WorkoutManager.App.Pages.TrainingLog.Dialogs.TrainingSessionDialog;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Structures.Collections.Common;
using WorkoutManager.App.Structures.Dialogs;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Service.Services;

namespace WorkoutManager.App.Pages.TrainingLog
{
    internal class TrainingLogPageViewModel : DialogModelBase
    {
        public static string DialogsIdentifier => "TrainingLogDialogs";

        private readonly TrainingSessionService _trainingSessionService;
        private readonly DialogFactory _dialogs;

        
        #region Commands

        public ICommand OpenAddSessionDialogCommand { get; }
        
        public ICommand OpenEditSessionDialogCommand { get; }
        
        public ICommand DeleteSessionCommand { get; }
        
        public ICommand OpenExportDialogCommand { get; }

        #endregion


        #region UI Properties

        public ObservableRangeCollection<TrainingSession> TrainingSessions { get; } = new WpfObservableRangeCollection<TrainingSession>(); 

        #endregion
        
        
        #region TrainingSessionLog

        private async void OpenAddSessionDialogAsync()
        {
            var bodyweight = _trainingSessionService.GetLastUsedBodyweight() ?? 0;
                    
            var trainingSession = new TrainingSession
            {
                Date = DateTime.Now,
                Bodyweight = bodyweight
            };

            var dialog = _dialogs.For<TrainingSessionDialogViewModel>(DialogsIdentifier);           
            dialog.Data.DialogTitle = "New session";
            dialog.Data.SubmitButtonTitle = "Create";
            dialog.Data.TrainingSession = TrainingSessionViewModel.FromModel(trainingSession);
            
            var dialogResult = await dialog.Show();

            if (dialogResult != DialogResult.Ok)
            {
                return;
            }

            trainingSession = dialog.Data.TrainingSession.ToModel();
            TrainingSessions.Add(trainingSession);
            _trainingSessionService.Create(trainingSession);
        }

        private async void OpenEditSessionDialogAsync(TrainingSession trainingSession)
        {
            var trainingSessionClone = trainingSession.Clone();

            var dialog = _dialogs.For<TrainingSessionDialogViewModel>(DialogsIdentifier);    
            dialog.Data.DialogTitle = "Modified session";
            dialog.Data.SubmitButtonTitle = "Save";
            dialog.Data.TrainingSession = TrainingSessionViewModel.FromModel(trainingSessionClone);
            
            var dialogResult = await dialog.Show();

            if (dialogResult != DialogResult.Ok)
            {
                return;
            }

            trainingSessionClone = dialog.Data.TrainingSession.ToModel();
            TrainingSessions.Replace(trainingSession, trainingSessionClone); 
            _trainingSessionService.Update(trainingSessionClone);
        }
        
        #endregion


        #region ExportDialog

        private void OpenExportDialog()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Text files|*.txt",
            };
            
            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                Task.Run(() => _trainingSessionService.ExportToFile(dialog.FileName));
            }
        }

        #endregion 
       

        private void LoadTrainingSessionsAsync()
            => Task.Run(() => TrainingSessions.AddRange(_trainingSessionService.GetAll()));
        
        private void DeleteSession(TrainingSession trainingSession)
        {
            TrainingSessions.Remove(trainingSession);

            Task.Run(() => _trainingSessionService.Delete(trainingSession));
        }
        
        public TrainingLogPageViewModel(TrainingSessionService trainingSessionService, DialogFactory dialogs)
        {
            _trainingSessionService = trainingSessionService;
            _dialogs = dialogs;

            OpenAddSessionDialogCommand = new Command(OpenAddSessionDialogAsync);
            OpenEditSessionDialogCommand = new Command<TrainingSession>(OpenEditSessionDialogAsync);
            DeleteSessionCommand = new Command<TrainingSession>(DeleteSession);
            
            OpenExportDialogCommand = new Command(OpenExportDialog);

            LoadTrainingSessionsAsync();
        }
    }
}