using System.Configuration;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Exercises.Models
{
    internal class MainWindowViewModel
    {
        public ExercisesPageViewModel ExercisesPage { get; }
        
        public MotionsPageViewModel MotionsPage { get; }
        
        public MuscleGroupsPageViewModel MuscleGroupsPage { get; }

        public MainWindowViewModel()
        {
            var dbFileName = ConfigurationManager.AppSettings["LiteDbFileName"];

            var exerciseRepository = new ExerciseRepository(dbFileName);
            var motionsRepository = new Repository<JointMotion>(dbFileName);
            var muscleGroupRepository = new MuscleGroupRepository(dbFileName);
            var muscleHeadRepository = new Repository<MuscleHead>(dbFileName);
            var exercisedMuscleRepository = new Repository<ExercisedMuscle>(dbFileName);
            
            ExercisesPage = new ExercisesPageViewModel(exerciseRepository, motionsRepository, muscleGroupRepository, exercisedMuscleRepository);
            MotionsPage = new MotionsPageViewModel(motionsRepository);
            MuscleGroupsPage = new MuscleGroupsPageViewModel(muscleGroupRepository, muscleHeadRepository);
        }
    }
}