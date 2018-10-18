using System.Configuration;
using PubSub.Core;
using Unity;
using Unity.Lifetime;
using WorkoutManager.App.Pages;
using WorkoutManager.App.Pages.Categories;
using WorkoutManager.App.Pages.Exercises;
using WorkoutManager.App.Pages.Muscles;
using WorkoutManager.App.Pages.Progress;
using WorkoutManager.App.Pages.TrainingLog;
using WorkoutManager.App.Pages.UserSettings;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract;
using WorkoutManager.Contract.Models.Categories;
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
            container.RegisterType<Hub>(new SingletonLifetimeManager());
            
            container.RegisterInstance(GetDatabaseConfiguration(), new SingletonLifetimeManager());
            container.RegisterType(typeof(Repository<>), new SingletonLifetimeManager());
            container.RegisterType<Repository<Exercise>, ExerciseRepository>(new SingletonLifetimeManager());
            container.RegisterType<Repository<SessionExercise>, SessionExerciseRepository>(new SingletonLifetimeManager());
            container.RegisterType<Repository<TrainingSession>, TrainingSessionRepository>(new SingletonLifetimeManager());
            container.RegisterType<Repository<Category>, CategoryRepository>(new SingletonLifetimeManager());
            
            container.RegisterType<ExerciseService>(new SingletonLifetimeManager());
            container.RegisterType<DatabaseService>(new SingletonLifetimeManager());
            container.RegisterType<TrainingSessionService>(new SingletonLifetimeManager());
            container.RegisterType<UserPreferencesService>(new SingletonLifetimeManager());
            
            container.RegisterType<ExercisesPageViewModel>(new SingletonLifetimeManager());
            container.RegisterType<MusclesPageViewModel>(new SingletonLifetimeManager());
            container.RegisterType<TrainingLogPageViewModel>(new SingletonLifetimeManager());
            container.RegisterType<UserPreferencesPageViewModel>(new SingletonLifetimeManager());
            container.RegisterType<ProgressPageViewModel>(new SingletonLifetimeManager());
            container.RegisterType<CategoryPageViewModel>(new SingletonLifetimeManager());
            
            container.RegisterType(typeof(ViewModelFactory<>), new SingletonLifetimeManager());
            container.RegisterType<WindowFactory>(new SingletonLifetimeManager());
            container.RegisterType(typeof(DialogFactory<,>), new SingletonLifetimeManager());
            
            container.RegisterType<MainWindow>(new SingletonLifetimeManager());
        }
    }
}