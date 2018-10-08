using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Force.DeepCloner;
using WorkoutManager.App.Pages.TrainingLog.Dialogs;
using WorkoutManager.App.Pages.TrainingLog.Models;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Repository;
using WorkoutManager.Service;

namespace WorkoutManager.App.Pages.TrainingLog
{
    internal class TrainingLogPageViewModel
    {
        public BulkObservableCollection<TrainingSession> TrainingSessions { get; } =
            new BulkObservableCollection<TrainingSession>();

        public ICommand OpenAddSessionDialog { get; }
        
        public ICommand OpenEditSessionDialog { get; }
        
        public ICommand DeleteSession { get; }
        
        private readonly TrainingSessionService _trainingSessionService;

        private void LoadTrainingSessionsAsync()
            => Task.Run(() => TrainingSessions.AddRange(_trainingSessionService.GetAll()));
        
        public TrainingLogPageViewModel(TrainingSessionService trainingSessionService, Repository<Exercise> exerciseRepository, UserPreferencesService userPreferencesService)
        {
            _trainingSessionService = trainingSessionService;

            TrainingSessions.ShapeView().OrderByDescending(session => session.Date).Apply();
                
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
                    var viewModel = new TrainingSessionDialogViewModel(trainingSessionClone, exerciseRepository, userPreferencesService)
                    {
                        SaveButtonTitle = "Save"
                    };

                    var dialogResult = DialogBuilder.Create<TrainingSessionDialog>().WithContext(viewModel).Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    TrainingSessions.Replace(session => Equals(session, trainingSessionClone), trainingSessionClone);
                    
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


                    var viewModel = new TrainingSessionDialogViewModel(trainingSession, exerciseRepository, userPreferencesService)
                    {
                        SaveButtonTitle = "Create"
                    };

                    var dialogResult = DialogBuilder.Create<TrainingSessionDialog>().WithContext(viewModel).Show();

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