using System.ComponentModel;
using System.Windows.Input;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Contract.Models.User;
using WorkoutManager.Repository;
using WorkoutManager.Service.Services;

namespace WorkoutManager.App.Pages.TrainingLog.Models
{
    internal class TrainingSessionDialogViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public TrainingSession TrainingSession
        {
            get => _trainingSession;
            set => SetField(ref _trainingSession, value);
        }

        public ViewModelFactory<SessionExerciseViewModel> ViewModelFactory { get; }
        
        public ObservedCollection<SessionExercise> SessionExercises { get; set; }
        
        public string SaveButtonTitle { get; set; }
        
        public BulkObservableCollection<Exercise> Exercises { get; } = new BulkObservableCollection<Exercise>();
        
        public ICommand AddExercise { get; } 
        
        public ICommand RemoveExercise { get; }

        public UserPreferences UserPreferences => _userPreferencesService.Load();
        
        private readonly Repository<Exercise> _exerciseRepository;
        private readonly UserPreferencesService _userPreferencesService;
        private TrainingSession _trainingSession;

        private void InitializeDataAsync()
        {
            Exercises.AddRange(_exerciseRepository.GetAll());
            OnPropertyChanged(nameof(Exercises));
        }
        
        public TrainingSessionDialogViewModel(Repository<Exercise> exerciseRepository, UserPreferencesService userPreferencesService, ViewModelFactory<SessionExerciseViewModel> viewModelFactory)
        {
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(TrainingSession))
                {
                    SessionExercises = new ObservedCollection<SessionExercise>(
                        TrainingSession.Exercises,
                        TrainingSession.AddExercise,
                        TrainingSession.RemoveExercise
                    );
                }
            };
            
            ViewModelFactory = viewModelFactory;
            _exerciseRepository = exerciseRepository;
            _userPreferencesService = userPreferencesService;
            
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
    }
}