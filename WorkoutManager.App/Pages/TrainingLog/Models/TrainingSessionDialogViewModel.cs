using System.ComponentModel;
using System.Windows.Input;
using WorkoutManager.App.Pages.Exercises.Models;
using WorkoutManager.App.Structures;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Contract.Models.User;
using WorkoutManager.Repository;
using WorkoutManager.Service;

namespace WorkoutManager.App.Pages.TrainingLog.Models
{
    internal class TrainingSessionDialogViewModel : INotifyPropertyChanged
    {
        public TrainingSession TrainingSession { get; }

        public ObservedCollection<SessionExercise> SessionExercises { get; }
        
        public string SaveButtonTitle { get; set; }
        
        public BulkObservableCollection<Exercise> Exercises { get; } = new BulkObservableCollection<Exercise>();
        
        public ICommand AddExercise { get; } 
        
        public ICommand RemoveExercise { get; }

        public UserPreferences UserPreferences => _userPreferencesService.Load();
        
        private readonly Repository<Exercise> _exerciseRepository;
        private readonly UserPreferencesService _userPreferencesService;
       
        private void InitializeDataAsync()
        {
            Exercises.AddRange(_exerciseRepository.GetAll());  
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }
        
        public TrainingSessionDialogViewModel(TrainingSession trainingSession, Repository<Exercise> exerciseRepository, UserPreferencesService userPreferencesService)
        {
            _exerciseRepository = exerciseRepository;
            _userPreferencesService = userPreferencesService;
            
            TrainingSession = trainingSession;

            SessionExercises = new ObservedCollection<SessionExercise>(
                trainingSession.Exercises,
                trainingSession.AddExercise,
                trainingSession.RemoveExercise
            );
            
            AddExercise = new Command<Exercise>(
                exercise =>
                {
                    var sessionExercise = new SessionExercise(exercise);
                    SessionExercises.Add(sessionExercise);
                });      

            RemoveExercise = new Command<SessionExercise>(
                exercise => { SessionExercises.Remove(exercise); }
            );
            
            InitializeDataAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}