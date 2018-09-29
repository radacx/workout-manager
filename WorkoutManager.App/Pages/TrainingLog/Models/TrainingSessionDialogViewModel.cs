using System;
using System.ComponentModel;
using System.Windows.Input;
using WorkoutManager.App.Pages.TrainingLog.Dialogs;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.ExerciseSet;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Pages.TrainingLog.Models
{
    internal class RemoveExerciseSetParameters
    {
        public SessionExercise Exercise { get; set; }
        
        public IExerciseSet Set { get; set; }
    }
    
    internal class TrainingSessionDialogViewModel : INotifyPropertyChanged
    {
        public TrainingSession TrainingSession { get; }

        public string SaveButtonTitle { get; set; }
        
        public BulkObservableCollection<Exercise> Exercises { get; } = new BulkObservableCollection<Exercise>();

        public ICommand OpenAddExerciseSetDialog { get; }
        
        public ICommand AddExercise { get; } 
        
        public ICommand RemoveExerciseSet { get; }
        
        public ICommand RemoveExercise { get; }
        
        private readonly Repository<Exercise> _exerciseRepository;
        private readonly DialogViewer<ExerciseSetDialog> _exerciseSetDialogViewer;
        
        private void InitializeDataAsync()
        {
            Exercises.AddRange(_exerciseRepository.GetAll());
            
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }

        private static IExerciseSet CreateSet(ContractionType type)
        {
            switch (type)
            {
                case ContractionType.Dynamic:
                    return new DynamicExerciseSet();
                
                case ContractionType.Isometric:
                    return new IsometricExerciseSet();
                
                default:
                    throw new ArgumentException($"Invalid contraction type: {type}");
            }
        }
        
        public TrainingSessionDialogViewModel(TrainingSession trainingSession, Repository<Exercise> exerciseRepository, DialogViewer<ExerciseSetDialog> exerciseSetDialogViewer)
        {
            _exerciseRepository = exerciseRepository;
            _exerciseSetDialogViewer = exerciseSetDialogViewer;
            
            TrainingSession = trainingSession;

            AddExercise = new Command<Exercise>(
                exercise =>
                {
                    var sessionExercise = new SessionExercise(exercise);
                    TrainingSession.AddExercise(sessionExercise);
                });
            
            OpenAddExerciseSetDialog = new Command<SessionExercise>(
                exercise =>
                {
                    var set = CreateSet(exercise.Exercise.ContractionType);
                    var viewModel = new ExerciseSetDialogViewModel(set);

                    var dialogResult = _exerciseSetDialogViewer.WithContext(viewModel).Show();
                    
                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    exercise.AddSet(set);
                });
            
            RemoveExerciseSet = new Command<RemoveExerciseSetParameters>(
                parameters => { parameters.Exercise.RemoveSet(parameters.Set); });

            RemoveExercise = new Command<SessionExercise>(
                exercise => { TrainingSession.RemoveExercise(exercise); }
            );
            
            InitializeDataAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}