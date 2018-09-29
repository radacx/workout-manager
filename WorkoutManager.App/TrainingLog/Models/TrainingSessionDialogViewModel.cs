using System;
using System.ComponentModel;
using System.Windows.Input;
using WorkoutManager.App.Misc;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.ExerciseSet;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Repository;

namespace WorkoutManager.App.TrainingLog.Models
{
    internal class RemoveExerciseSetParameters
    {
        public Contract.Models.Sessions.SessionExercise Exercise { get; set; }
        
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
        
        public TrainingSessionDialogViewModel(TrainingSession trainingSession, Repository<Exercise> exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
            
            TrainingSession = trainingSession;

            AddExercise = new Command<Exercise>(
                exercise =>
                {
                    var sessionExercise = new Contract.Models.Sessions.SessionExercise(exercise);
                    TrainingSession.AddExercise(sessionExercise);
                });
            
            OpenAddExerciseSetDialog = new Command<Contract.Models.Sessions.SessionExercise>(
                exercise =>
                {
                    var set = CreateSet(exercise.Exercise.ContractionType);
                    
                    var viewModel = new ExerciseSetDialogViewModel(set);
                    var dialog = new ExerciseSetDialog { DataContext = viewModel };

                    var dialogResult = dialog.ShowDialog();

                    if (!dialogResult.HasValue || !dialogResult.Value)
                    {
                        return;
                    }

                    exercise.AddSet(set);
                });
            
            RemoveExerciseSet = new Command<RemoveExerciseSetParameters>(
                parameters => { parameters.Exercise.RemoveSet(parameters.Set); });

            RemoveExercise = new Command<Contract.Models.Sessions.SessionExercise>(
                exercise => { TrainingSession.RemoveExercise(exercise); }
            );
            
            InitializeDataAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}