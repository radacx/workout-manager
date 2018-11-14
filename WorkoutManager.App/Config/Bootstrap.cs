using System;
using System.Configuration;
using PubSub.Core;
using Unity;
using Unity.Lifetime;
using WorkoutManager.Contract;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Repository;
using WorkoutManager.Repository.Repositories;

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
            var bootstrapper = new Bootstrapper(container);

            bootstrapper.RegisterInstance(GetDatabaseConfiguration())
                
                .RegisterType<Hub>()
                
                .RegisterType<Repository<Exercise>, ExerciseRepository>()
                .RegisterType<Repository<TrainingSession>, TrainingSessionRepository>();
        }
        
        private class Bootstrapper
        {
            private readonly IUnityContainer _container;

            public Bootstrapper(IUnityContainer container)
            {
                _container = container;
            }

            public Bootstrapper RegisterType<T>()
            {
                _container.RegisterType<T>(new SingletonLifetimeManager());
            
                return this;
            }

            public Bootstrapper RegisterType(Type type)
            {
                _container.RegisterType(type, new SingletonLifetimeManager());

                return this;
            }
            
            public Bootstrapper RegisterType<TResolveFrom, TResolveTo>()
                where TResolveTo : TResolveFrom
            {
                _container.RegisterType<TResolveFrom, TResolveTo>(new SingletonLifetimeManager());
                
                return this;
            }
            
            public Bootstrapper RegisterInstance<TInstance>(TInstance instance)
            {
                _container.RegisterInstance(instance, new SingletonLifetimeManager());
                
                return this;
            }
        }
    }
}