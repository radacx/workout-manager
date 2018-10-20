using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Force.DeepCloner;
using Microsoft.Win32;
using WorkoutManager.App.Pages.TrainingLog.Dialogs;
using WorkoutManager.App.Pages.TrainingLog.Models;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Service.Services;

namespace WorkoutManager.App.Pages.TrainingLog
{
    internal class TrainingLogPageViewModel : ViewModelBase
    {
        public BulkObservableCollection<TrainingSession> TrainingSessions { get; } =
            new BulkObservableCollection<TrainingSession>();

        public ICommand OpenAddSessionDialog { get; }
        
        public ICommand OpenEditSessionDialog { get; }
        
        public ICommand DeleteSession { get; }
        
        public ICommand Export { get; }
        
        private readonly TrainingSessionService _trainingSessionService;

        private void LoadTrainingSessionsAsync()
            => Task.Run(() => TrainingSessions.AddRange(_trainingSessionService.GetAll()));
        
        public TrainingLogPageViewModel(TrainingSessionService trainingSessionService, DialogFactory<TrainingSessionDialog, TrainingSessionDialogViewModel> trainingSessionDialogFactory)
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
                trainingSession =>
                {
                    var trainingSessionClone = trainingSession.DeepClone();

                    var dialog = trainingSessionDialogFactory.Get();
                    dialog.Data.TrainingSession = trainingSessionClone;
                    dialog.Data.SaveButtonTitle = "Save";
                    
                    var dialogResult = dialog.Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    TrainingSessions.Replace(trainingSession, trainingSessionClone);
                    
                    Task.Run(() => _trainingSessionService.Update(trainingSessionClone));
                });

            OpenAddSessionDialog = new Command(
                () =>
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

                    var dialog = trainingSessionDialogFactory.Get();
                    dialog.Data.TrainingSession = trainingSession;
                    dialog.Data.SaveButtonTitle = "Create";

                    var dialogResult = dialog.Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    TrainingSessions.Add(trainingSession);
                    
                    Task.Run(() => _trainingSessionService.Create(trainingSession));
                });

            LoadTrainingSessionsAsync();
        }
    }
}