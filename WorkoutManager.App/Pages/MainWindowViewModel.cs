using System.Configuration;
using WorkoutManager.App.Pages.Exercises.Dialogs;
using WorkoutManager.App.Pages.Exercises.Models;
using WorkoutManager.App.Pages.Motions.Dialogs;
using WorkoutManager.App.Pages.Motions.Models;
using WorkoutManager.App.Pages.MuscleGroups.Dialogs;
using WorkoutManager.App.Pages.MuscleGroups.Models;
using WorkoutManager.App.Pages.TrainingLog.Dialogs;
using WorkoutManager.App.Pages.TrainingLog.Models;
using WorkoutManager.App.Pages.UserSettings.Models;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.ExerciseSet;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Contract.Models.User;
using WorkoutManager.Repository;
using WorkoutManager.Service;

namespace WorkoutManager.App.Pages
{
    internal class MainWindowViewModel
    {
        public ExercisesPageViewModel ExercisesPage { get; }
        
        public MotionsPageViewModel MotionsPage { get; }
        
        public MuscleGroupsPageViewModel MuscleGroupsPage { get; }

        public TrainingLogPageViewModel TrainingLogPage { get; }
        
        public UserPreferencesPageViewModel UserPreferencesPage { get; }
        
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
            var userPreferencesRepository = new Repository<UserPreferences>(dbFileName);
            
            var exerciseService = new ExerciseService(exerciseRepository, exercisedMuscleRepository);
            var muscleGroupService = new MuscleGroupService(muscleGroupRepository, muscleHeadRepository);
            var trainingSessionService = new TrainingSessionService(trainingSessionRepository, sessionExerciseRepository, exerciseSetRepository);
            var userPreferencesService = new UserPreferencesService(userPreferencesRepository);
            
            var exerciseDialogViewer = new DialogViewer<ExerciseDialog>();
            var motionDialogViewer = new DialogViewer<JointMotionDialog>();
            var muscleGroupDialogViewer = new DialogViewer<MuscleGroupDialog>();
            var muscleHeadDialogViewer = new DialogViewer<MuscleHeadDialog>();
            var trainingSessionDialogViewer = new DialogViewer<TrainingSessionDialog>();
            var exerciseSetDialogViewer = new DialogViewer<ExerciseSetDialog>();
            
            ExercisesPage = new ExercisesPageViewModel(exerciseService, motionsRepository, muscleGroupRepository, exerciseDialogViewer);
            MotionsPage = new MotionsPageViewModel(motionsRepository, motionDialogViewer);
            MuscleGroupsPage = new MuscleGroupsPageViewModel(muscleGroupService, muscleGroupDialogViewer, muscleHeadDialogViewer);
            TrainingLogPage = new TrainingLogPageViewModel(trainingSessionService, exerciseRepository, trainingSessionDialogViewer, userPreferencesService, exerciseSetDialogViewer);
            UserPreferencesPage = new UserPreferencesPageViewModel(userPreferencesService);
        }
    }
}