using System.Configuration;
using WorkoutManager.App.TrainingLog.Models;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.ExerciseSet;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Exercises.Models
{
    internal class MainWindowViewModel
    {
        public ExercisesPageViewModel ExercisesPage { get; }
        
        public MotionsPageViewModel MotionsPage { get; }
        
        public MuscleGroupsPageViewModel MuscleGroupsPage { get; }

        public TrainingLogPageViewModel TrainingLogPage { get; }
        
        public MainWindowViewModel()
        {
            var dbFileName = ConfigurationManager.AppSettings["LiteDbFileName"];

            var exerciseRepository = new ExerciseRepository(dbFileName);
            var motionsRepository = new Repository<JointMotion>(dbFileName);
            var muscleGroupRepository = new MuscleGroupRepository(dbFileName);
            var muscleHeadRepository = new Repository<MuscleHead>(dbFileName);
            var exercisedMuscleRepository = new Repository<ExercisedMuscle>(dbFileName);
            var trainingSessionRepository = new TrainingSessionRepository(dbFileName);
            var sessionExerciseRepository = new Repository<SessionExercise>(dbFileName);
            var exerciseSetRepository = new Repository<IExerciseSet>(dbFileName);
            
            ExercisesPage = new ExercisesPageViewModel(exerciseRepository, motionsRepository, muscleGroupRepository, exercisedMuscleRepository);
            MotionsPage = new MotionsPageViewModel(motionsRepository);
            MuscleGroupsPage = new MuscleGroupsPageViewModel(muscleGroupRepository, muscleHeadRepository);
            TrainingLogPage = new TrainingLogPageViewModel(trainingSessionRepository, exerciseRepository, sessionExerciseRepository, exerciseSetRepository);
        }
    }
}