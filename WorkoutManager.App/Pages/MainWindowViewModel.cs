using WorkoutManager.App.Pages.Categories;
using WorkoutManager.App.Pages.Exercises;
using WorkoutManager.App.Pages.Motions;
using WorkoutManager.App.Pages.MuscleGroups;
using WorkoutManager.App.Pages.Progress;
using WorkoutManager.App.Pages.TrainingLog;
using WorkoutManager.App.Pages.UserSettings;
using WorkoutManager.App.Structures;

namespace WorkoutManager.App.Pages
{
    internal class MainWindowViewModel : IViewModel
    {
        public ExercisesPageViewModel ExercisesPage { get; }
        
        public MotionsPageViewModel MotionsPage { get; }
        
        public MuscleGroupsPageViewModel MuscleGroupsPage { get; }

        public TrainingLogPageViewModel TrainingLogPage { get; }
        
        public CategoryPageViewModel CategoryPage { get; }
        
        public ProgressPageViewModel ProgressPage { get; }
        
        public UserPreferencesPageViewModel UserPreferencesPage { get; }
        
        public MainWindowViewModel(
            ExercisesPageViewModel exercisesPage,
            MotionsPageViewModel motionsPage,
            MuscleGroupsPageViewModel muscleGroupsPage,
            TrainingLogPageViewModel trainingLogPage,
            ProgressPageViewModel progressPage,
            UserPreferencesPageViewModel userPreferencesPage,
            CategoryPageViewModel categoryPage
        )
        {
            ExercisesPage = exercisesPage;
            MotionsPage = motionsPage;
            MuscleGroupsPage = muscleGroupsPage;
            TrainingLogPage = trainingLogPage;
            ProgressPage = progressPage;
            UserPreferencesPage = userPreferencesPage;
            CategoryPage = categoryPage;
        }
    }
}