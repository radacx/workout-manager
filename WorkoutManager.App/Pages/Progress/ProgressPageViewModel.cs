using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutManager.App.Structures;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Pages.Progress
{
    internal class ProgressPageViewModel : ViewModelBase
    {
        private readonly Repository<Exercise> _exerciseRepository;
        private readonly Repository<TrainingSession> _trainingSessionRepository;
        private Exercise _selectedExercise;

        public BulkObservableCollection<TrainingSession> TrainingSessions { get; } = new BulkObservableCollection<TrainingSession>();
        
        public BulkObservableCollection<Exercise> Exercises { get; } = new BulkObservableCollection<Exercise>();

        public Exercise SelectedExercise
        {
            get => _selectedExercise;
            set => SetField(ref _selectedExercise, value);
        }

        public DateTime DateFrom { get; set; }
        
        public DateTime DateTo { get; }
        
        public ICommand Refresh { get; }

        private void LoadData()
        {
            Exercises.AddRange(_exerciseRepository.GetAll());
            TrainingSessions.AddRange(_trainingSessionRepository.GetAll());
        }

        private void ClearData()
        {
            Exercises.Clear();
            TrainingSessions.Clear();
        }
        
        public ProgressPageViewModel(Repository<Exercise> exerciseRepository, Repository<TrainingSession> trainingSessionRepository)
        {
            _exerciseRepository = exerciseRepository;
            _trainingSessionRepository = trainingSessionRepository;

            Task.Run(() => LoadData());

            Refresh = new Command(
                () =>
                {
                    Task.Run(
                        () =>
                        {
                            var previousSelection = SelectedExercise;
                            ClearData();
                            LoadData();

                            if (!Exercises.Contains(previousSelection))
                            {
                                return;
                            }

                            SelectedExercise = Exercises.First(exercise => exercise.Equals(previousSelection));
                            OnPropertyChanged(nameof(SelectedExercise));
                        }
                    );
                });
        }
    }
}