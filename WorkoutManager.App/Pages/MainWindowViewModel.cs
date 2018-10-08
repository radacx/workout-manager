using System.Configuration;
using WorkoutManager.App.Pages.Exercises;
using WorkoutManager.App.Pages.Exercises.Dialogs;
using WorkoutManager.App.Pages.Motions;
using WorkoutManager.App.Pages.Motions.Dialogs;
using WorkoutManager.App.Pages.MuscleGroups;
using WorkoutManager.App.Pages.MuscleGroups.Dialogs;
using WorkoutManager.App.Pages.Progress;
using WorkoutManager.App.Pages.TrainingLog;
using WorkoutManager.App.Pages.TrainingLog.Dialogs;
using WorkoutManager.App.Pages.UserSettings;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.ExerciseSet;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Contract.Models.User;
using WorkoutManager.Repository;
using WorkoutManager.Repository.Repositories;
using WorkoutManager.Service;

namespace WorkoutManager.App.Pages
{
    internal class MainWindowViewModel
    {
        public ExercisesPageViewModel ExercisesPage { get; }
        
        public MotionsPageViewModel MotionsPage { get; }
        
        public MuscleGroupsPageViewModel MuscleGroupsPage { get; }

        public TrainingLogPageViewModel TrainingLogPage { get; }
        
        public ProgressPageViewModel ProgressPage { get; }
        
        public UserPreferencesPageViewModel UserPreferencesPage { get; }
        
        public MainWindowViewModel()
        {
            var dbFileName = ConfigurationManager.AppSettings["LiteDbFileName"];

            var exerciseRepository = new ExerciseRepository(dbFileName);
            var motionsRepository = new Repository<JointMotion>(dbFileName);
            var muscleGroupRepository = new MuscleGroupRepository(dbFileName);
            var muscleHeadRepository = new Repository<MuscleHead>(dbFileName);
            var exercisedMuscleRepository = new ExercisedMuscleRepository(dbFileName);
            var trainingSessionRepository = new TrainingSessionRepository(dbFileName);
            var sessionExerciseRepository = new SessionExerciseRepository(dbFileName);
            var exerciseSetRepository = new Repository<IExerciseSet>(dbFileName);
            var userPreferencesRepository = new Repository<UserPreferences>(dbFileName);
            
            var exerciseService = new ExerciseService(exerciseRepository, exercisedMuscleRepository);
            var muscleGroupService = new MuscleGroupService(muscleGroupRepository, muscleHeadRepository);
            var trainingSessionService = new TrainingSessionService(trainingSessionRepository, sessionExerciseRepository, exerciseSetRepository);
            var userPreferencesService = new UserPreferencesService(userPreferencesRepository);
            var databaseService = new DatabaseService(dbFileName);
            
            
            ExercisesPage = new ExercisesPageViewModel(exerciseService, motionsRepository, muscleGroupRepository);
            MotionsPage = new MotionsPageViewModel(motionsRepository);
            MuscleGroupsPage = new MuscleGroupsPageViewModel(muscleGroupService);
            TrainingLogPage = new TrainingLogPageViewModel(trainingSessionService, exerciseRepository, userPreferencesService);
            UserPreferencesPage = new UserPreferencesPageViewModel(userPreferencesService, databaseService);
            ProgressPage = new ProgressPageViewModel(exerciseRepository, trainingSessionRepository);
        }
    }
}