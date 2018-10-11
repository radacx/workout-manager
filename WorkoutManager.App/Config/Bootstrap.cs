using System.Configuration;
using Unity;
using WorkoutManager.App.Pages;
using WorkoutManager.App.Pages.Exercises;
using WorkoutManager.App.Pages.Motions;
using WorkoutManager.App.Pages.MuscleGroups;
using WorkoutManager.App.Pages.Progress;
using WorkoutManager.App.Pages.TrainingLog;
using WorkoutManager.App.Pages.UserSettings;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Repository;
using WorkoutManager.Repository.Repositories;
using WorkoutManager.Service.Services;

namespace WorkoutManager.App.Config
{
    internal static class Bootstrap
    {
        private static DatabaseConfiguration GetDatabaseConfiguration()
        {
            var fileName = ConfigurationManager.AppSettings["LiteDbFileName"];

            return new DatabaseConfiguration
            {
                FileName = fileName
            };
        }

        public static void Register(IUnityContainer container)
        {
            container.RegisterInstance(GetDatabaseConfiguration());
            
            container.RegisterType(typeof(Repository<>));
            container.RegisterType<Repository<Exercise>, ExerciseRepository>();
            container.RegisterType<Repository<ExercisedMuscle>, ExercisedMuscleRepository>();
            container.RegisterType<Repository<MuscleGroup>, MuscleGroupRepository>();
            container.RegisterType<Repository<SessionExercise>, SessionExerciseRepository>();
            container.RegisterType<Repository<TrainingSession>, TrainingSessionRepository>();
            
            container.RegisterType<ExerciseService>();
            container.RegisterType<DatabaseService>();
            container.RegisterType<MuscleGroupService>();
            container.RegisterType<TrainingSessionService>();
            container.RegisterType<UserPreferencesService>();

            container.RegisterType<ExercisesPageViewModel>();
            container.RegisterType<MotionsPageViewModel>();
            container.RegisterType<MuscleGroupsPageViewModel>();
            container.RegisterType<TrainingLogPageViewModel>();
            container.RegisterType<UserPreferencesPageViewModel>();
            container.RegisterType<ProgressPageViewModel>();

            container.RegisterType(typeof(ViewModelFactory<>));
            container.RegisterType<WindowFactory>();
            container.RegisterType(typeof(DialogFactory<,>));
            
            container.RegisterType<MainWindow>();
        }
    }
}