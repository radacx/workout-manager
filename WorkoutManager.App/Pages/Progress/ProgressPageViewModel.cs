using System;
using System.Threading.Tasks;
using WorkoutManager.App.Structures;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Pages.Progress
{
    internal class ProgressPageViewModel
    {
        private readonly Repository<Exercise> _exerciseRepository;
        private readonly Repository<TrainingSession> _trainingSessionRepository;
        
        public BulkObservableCollection<TrainingSession> TrainingSessions { get; } = new BulkObservableCollection<TrainingSession>();
        
        public BulkObservableCollection<Exercise> Exercises { get; } = new BulkObservableCollection<Exercise>();
        
        public Exercise SelectedExercise { get; set; }
        
        public DateTime DateFrom { get; set; }
        
        public DateTime DateTo { get; }
        
        private void InitializeData()
        {
            Exercises.AddRange(_exerciseRepository.GetAll());
            TrainingSessions.AddRange(_trainingSessionRepository.GetAll());
        }
        
        public ProgressPageViewModel(Repository<Exercise> exerciseRepository, Repository<TrainingSession> trainingSessionRepository)
        {
            _exerciseRepository = exerciseRepository;
            _trainingSessionRepository = trainingSessionRepository;

            Task.Run(() => InitializeData());
        }
    }
}