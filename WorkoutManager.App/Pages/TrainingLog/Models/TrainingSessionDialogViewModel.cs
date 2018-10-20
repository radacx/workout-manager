using System.Linq;
using System.Windows.Input;
using PubSub.Core;
using WorkoutManager.App.Events;
using WorkoutManager.App.Pages.TrainingLog.Dialogs;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.ExerciseSet;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Contract.Models.User;
using WorkoutManager.Repository;
using WorkoutManager.Service.Services;

namespace WorkoutManager.App.Pages.TrainingLog.Models
{
    internal class TrainingSessionDialogViewModel : ViewModelBase
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
        
        public ICommand AddExerciseSet { get; } 
        
        public ICommand RemoveExercise { get; }

        public UserPreferences UserPreferences => _userPreferencesService.Load();
        
        private readonly Repository<Exercise> _exerciseRepository;
        private readonly UserPreferencesService _userPreferencesService;
        private TrainingSession _trainingSession;

        private void InitializeDataAsync()
        {
            Exercises.AddRange(_exerciseRepository.GetAll().OrderBy(exercise => exercise.Name));
            OnPropertyChanged(nameof(Exercises));
        }
        
        public TrainingSessionDialogViewModel(Repository<Exercise> exerciseRepository, UserPreferencesService userPreferencesService, ViewModelFactory<SessionExerciseViewModel> viewModelFactory, DialogFactory<ExerciseSetDialog, ExerciseSetDialogViewModel> exerciseSetDialogFactory, Hub eventAggregator)
        {
            IExerciseSet OpenAddExerciseSet(SessionExercise sessionExercise)
            {
                var set = SetCreator.Create(sessionExercise.Exercise.ContractionType);

                var dialog = exerciseSetDialogFactory.Get();
                dialog.Data.ExerciseSet = set;
                dialog.Data.SaveButtonTitle = "Create";

                var dialogResult = dialog.Show();

                return dialogResult != DialogResult.Ok ? null : set;
            }
            
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
            
            AddExerciseSet = new Command<Exercise>(
                exercise =>
                {
                    var lastExercise = SessionExercises.LastOrDefault();

                    if (lastExercise == null || !lastExercise.Exercise.Equals(exercise))
                    {
                        lastExercise = new SessionExercise(exercise);

                        var set = OpenAddExerciseSet(lastExercise);

                        if (set == null)
                        {
                            return;
                        }

                        lastExercise.AddSet(set);
                        SessionExercises.Add(lastExercise);
                    }
                    else
                    {
                        eventAggregator.Publish(new SessionExerciseReAddedEvent(lastExercise));
                    }
                });      

            RemoveExercise = new Command<SessionExercise>(
                exercise => { SessionExercises.Remove(exercise); }
            );
            
            InitializeDataAsync();
        }
    }
}