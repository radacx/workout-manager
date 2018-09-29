using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutManager.App.Misc;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Repository;
using WorkoutManager.Service;

namespace WorkoutManager.App.TrainingLog.Models
{
    internal class TrainingLogPageViewModel
    {
        public BulkObservableCollection<TrainingSession> TrainingSessions { get; } = new BulkObservableCollection<TrainingSession>();

        public ICommand OpenAddSessionDialog { get; }
        
        public ICommand OpenEditSessionDialog { get; }
        
        public ICommand DeleteSession { get; }

        private readonly TrainingSessionService _trainingSessionService;
        
        private void LoadTrainingSessionsAsync()
            => Task.Run(() => TrainingSessions.AddRange(_trainingSessionService.GetAll()));

        public TrainingLogPageViewModel(TrainingSessionService trainingSessionService, Repository<Exercise> exerciseRepository)
        {
            _trainingSessionService = trainingSessionService;
            
            DeleteSession = new Command<TrainingSession>(session =>
                {
                    TrainingSessions.Remove(session);
                    
                    Task.Run(() => _trainingSessionService.Delete(session));
                }
            );

            OpenEditSessionDialog = new Command<TrainingSession>(
                trainingSession =>
                {
                    var trainingSessionClone = trainingSession.Clone();
                    var viewModel = new TrainingSessionDialogViewModel(trainingSessionClone, exerciseRepository)
                    {
                        SaveButtonTitle = "Save"
                    };

                    var dialog = new TrainingSessionDialog()
                    {
                        DataContext = viewModel
                    };

                    var dialogResult = dialog.ShowDialog();

                    if (!dialogResult.HasValue || !dialogResult.Value)
                    {
                        return;
                    }

                    TrainingSessions.Replace(session => Equals(session, trainingSessionClone), trainingSessionClone);

                    Task.Run(() => _trainingSessionService.Update(trainingSessionClone));
                });

            OpenAddSessionDialog = new Command(
                () =>
                {
                    var trainingSession = new TrainingSession();
                    var viewModel = new TrainingSessionDialogViewModel(trainingSession, exerciseRepository)
                    {
                        SaveButtonTitle = "Create"
                    };

                    var dialog = new TrainingSessionDialog()
                    {
                        DataContext = viewModel
                    };

                    var dialogResult = dialog.ShowDialog();

                    if (!dialogResult.HasValue || !dialogResult.Value)
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