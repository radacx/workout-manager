using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Force.DeepCloner;
using Microsoft.Win32;
using WorkoutManager.App.Pages.TrainingLog.Models;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils.Dialogs;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Service.Services;

namespace WorkoutManager.App.Pages.TrainingLog
{
    internal class TrainingLogPageViewModel : DialogModelBase
    {
        public string SessionDialogIdentifier => "SessionDialog";
        
        public ObservableRangeCollection<TrainingSession> TrainingSessions { get; } = new WpfObservableRangeCollection<TrainingSession>();

        public ICommand OpenAddSessionDialog { get; }
        
        public ICommand OpenEditSessionDialog { get; }
        
        public ICommand DeleteSession { get; }
        
        public ICommand Export { get; }
        
        private readonly TrainingSessionService _trainingSessionService;

        private void LoadTrainingSessionsAsync()
            => Task.Run(() => TrainingSessions.AddRange(_trainingSessionService.GetAll()));
        
        public TrainingLogPageViewModel(TrainingSessionService trainingSessionService, DialogViewer dialogViewer)
        {
            _trainingSessionService = trainingSessionService;

            TrainingSessions.ShapeView().OrderByDescending(session => session.Date).Apply();

            Export = new Command(
                () =>
                {
                    var dialog = new SaveFileDialog()
                    {
                        Filter = "Text files|*.txt"
                    };
                    var result = dialog.ShowDialog();

                    if (result.HasValue && result.Value)
                    {
                        Task.Run(() => _trainingSessionService.ExportToFile(dialog.FileName));
                    }
                });
            
            DeleteSession = new Command<TrainingSession>(session =>
                {
                    TrainingSessions.Remove(session);

                    Task.Run(() => _trainingSessionService.Delete(session));
                }
            );
                
            OpenEditSessionDialog = new Command<TrainingSession>(
                async trainingSession =>
                {
                    var trainingSessionClone = trainingSession.DeepClone();

                    var dialog = dialogViewer.For<TrainingSessionDialogViewModel>(SessionDialogIdentifier);
                    dialog.Data.TrainingSession = trainingSessionClone;
                    dialog.Data.SubmitButtonTitle = "Save";
                    dialog.Data.DialogTitle = "Modified session";
                    
                    var dialogResult = await dialog.Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    TrainingSessions.Replace(trainingSession, trainingSessionClone); 
                    _trainingSessionService.Update(trainingSessionClone);
                });

            OpenAddSessionDialog = new Command(
                async () =>
                {
                    var bodyweight = _trainingSessionService.GetAll()
                            .OrderByDescending(session => session.Date)
                            .FirstOrDefault()
                            ?.Bodyweight
                        ?? 0;
                    
                    var trainingSession = new TrainingSession
                    {
                        Date = DateTime.Now,
                        Bodyweight = bodyweight
                    };

                    var dialog = dialogViewer.For<TrainingSessionDialogViewModel>(SessionDialogIdentifier);
                    dialog.Data.TrainingSession = trainingSession;
                    dialog.Data.SubmitButtonTitle = "Create";
                    dialog.Data.DialogTitle = "New session";
                    
                    var dialogResult = await dialog.Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    TrainingSessions.Add(trainingSession);
                    _trainingSessionService.Create(trainingSession);
                });

            LoadTrainingSessionsAsync();
        }
    }
}