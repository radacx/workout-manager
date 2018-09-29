using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutManager.App.Misc;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.ExerciseSet;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Repository;

namespace WorkoutManager.App.TrainingLog.Models
{
    internal class TrainingLogPageViewModel
    {
        private readonly Repository<TrainingSession> _trainingSessionRepository;

        private readonly Repository<Contract.Models.Sessions.SessionExercise> _sessionExerciseRepository;

        private readonly Repository<IExerciseSet> _exerciseSetRepository;
        
        public BulkObservableCollection<TrainingSession> TrainingSessions { get; } = new BulkObservableCollection<TrainingSession>();

        public ICommand OpenAddSessionDialog { get; }
        
        public ICommand OpenEditSessionDialog { get; }
        
        public ICommand DeleteSession { get; }
        
        private void LoadTrainingSessionsAsync()
            => Task.Run(() => TrainingSessions.AddRange(_trainingSessionRepository.GetAll()));

        private void DeleteTrainingSession(TrainingSession session)
        {
            TrainingSessions.Remove(session);

            Task.Run(() => _trainingSessionRepository.Delete(session));
        }

        private void UpdateTrainingSession(TrainingSession session)
        {
            var newSets = session.Exercises.SelectMany(exercise => exercise.Sets).Where(set => set.Id == 0);
            var newExercises = session.Exercises.Where(exercise => exercise.Id == 0);
            
            _exerciseSetRepository.CreateRange(newSets);
            _sessionExerciseRepository.CreateRange(newExercises);
            _trainingSessionRepository.Update(session);
        }

        private void CreateTrainingSession(TrainingSession session)
        {
            var newSets = session.Exercises.SelectMany(exercise => exercise.Sets);
            var newExercises = session.Exercises;

            _exerciseSetRepository.CreateRange(newSets);
            _sessionExerciseRepository.CreateRange(newExercises);
            _trainingSessionRepository.Create(session);
        }

        public TrainingLogPageViewModel(Repository<TrainingSession> trainingSessionRepository, Repository<Exercise> exerciseRepository, Repository<Contract.Models.Sessions.SessionExercise> sessionExerciseRepository, Repository<IExerciseSet> exerciseSetRepository)
        {
            _trainingSessionRepository = trainingSessionRepository;
            _sessionExerciseRepository = sessionExerciseRepository;
            _exerciseSetRepository = exerciseSetRepository;
            
            DeleteSession = new Command<TrainingSession>(DeleteTrainingSession);

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

                    Task.Run(() => UpdateTrainingSession(trainingSessionClone));
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

                    Task.Run(() => CreateTrainingSession(trainingSession));
                });

            LoadTrainingSessionsAsync();
        }
    }
}