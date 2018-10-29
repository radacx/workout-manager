using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PubSub.Core;
using WorkoutManager.App.Events;
using WorkoutManager.App.Pages.TrainingLog.Dialogs;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils;
using WorkoutManager.App.Utils.Dialogs;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Exercises.Sets;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Contract.Models.User;
using WorkoutManager.Repository;
using WorkoutManager.Service.Services;

namespace WorkoutManager.App.Pages.TrainingLog.Models
{
    internal class TrainingSessionDialogSelector : DataTemplateSelector
    {
        public const string AddTrainingSessionExerciseDialogDataTemplateIdentifier = "AddTrainingSessionExerciseDialogDataTemplateIdentifier";
        public const string AddExerciseSetToNewTrainingSessionDialogDataTemplateIdentifier = "AddExerciseSetToNewTrainingSessionDialogDataTemplateIdentifier";
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item) {
                case AddTrainingSessionExerciseDialogViewModel _:

                    return Application.Current.MainWindow?.FindResource(AddTrainingSessionExerciseDialogDataTemplateIdentifier) as DataTemplate;
                case ExerciseSetDialogViewModel _:

                    return Application.Current.MainWindow?.FindResource(AddExerciseSetToNewTrainingSessionDialogDataTemplateIdentifier) as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    } 
    
    internal class TrainingSessionDialogViewModel : DialogModelBase
    {
        public string TrainingSessionDialogsIdentifier => "TrainingSessionDialogs";

        public TrainingSession TrainingSession
        {
            get => _trainingSession;
            set => SetField(ref _trainingSession, value);
        }

        public ViewModelFactory ViewModelFactory { get; }
        
        public ObservedCollection<SessionExercise> SessionExercises { get; set; }
        
        public ICommand AddSessionExercise { get; } 
        
        public ICommand RemoveExercise { get; }

        public UserPreferences UserPreferences => _userPreferencesService.Load();
        
        private readonly Repository<Exercise> _exerciseRepository;
        private readonly UserPreferencesService _userPreferencesService;
        private TrainingSession _trainingSession;

        private IEnumerable<Exercise> _exercises;
        
        private void InitializeDataAsync()
        {
            _exercises = _exerciseRepository.GetAll().OrderBy(exercise => exercise.Name);
        }
        
        public TrainingSessionDialogViewModel(Repository<Exercise> exerciseRepository, UserPreferencesService userPreferencesService, ViewModelFactory viewModelFactory, DialogViewer dialogViewer, Hub eventAggregator)
        {
            async Task<ExerciseSet> OpenAddExerciseSet(SessionExercise sessionExercise)
            {
                var set = SetCreator.Create(sessionExercise.Exercise.ContractionType);

                var dialog = dialogViewer.For<ExerciseSetDialogViewModel>(TrainingSessionDialogsIdentifier);
                dialog.Data.ExerciseSet = set;
                dialog.Data.SubmitButtonTitle = "Create";
                dialog.Data.DialogTitle = "Add set";
                
                var dialogResult = await dialog.Show();

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
            
            AddSessionExercise = new Command(
                async () =>
                {
                    var dialog = dialogViewer.For<AddTrainingSessionExerciseDialogViewModel>(TrainingSessionDialogsIdentifier);

                    dialog.Data.SubmitButtonTitle = "Create";
                    dialog.Data.DialogTitle = "New session exercise";
                    dialog.Data.Exercises = _exercises;

                    var dialogResult = await dialog.Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    var selectedExercise = dialog.Data.SelectedExercise;
                    
                    var lastExercise = SessionExercises.LastOrDefault();

                    if (lastExercise == null || !lastExercise.Exercise.Equals(selectedExercise))
                    {
                        lastExercise = new SessionExercise(selectedExercise);

                        var set = await OpenAddExerciseSet(lastExercise);

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