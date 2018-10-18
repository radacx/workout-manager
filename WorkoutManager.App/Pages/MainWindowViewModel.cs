using WorkoutManager.App.Pages.Categories;
using WorkoutManager.App.Pages.Exercises;
using WorkoutManager.App.Pages.Muscles;
using WorkoutManager.App.Pages.Progress;
using WorkoutManager.App.Pages.TrainingLog;
using WorkoutManager.App.Pages.UserSettings;
using WorkoutManager.App.Structures;

namespace WorkoutManager.App.Pages
{
    internal class MainWindowViewModel : IViewModel
    {
        public ExercisesPageViewModel ExercisesPage { get; }
        
        public MusclesPageViewModel MusclesPage { get; }

        public TrainingLogPageViewModel TrainingLogPage { get; }
        
        public CategoryPageViewModel CategoryPage { get; }
        
        public ProgressPageViewModel ProgressPage { get; }
        
        public UserPreferencesPageViewModel UserPreferencesPage { get; }
        
        public MainWindowViewModel(
            ExercisesPageViewModel exercisesPage,
            MusclesPageViewModel musclesPage,
            TrainingLogPageViewModel trainingLogPage,
            ProgressPageViewModel progressPage,
            UserPreferencesPageViewModel userPreferencesPage,
            CategoryPageViewModel categoryPage
        )
        {
            ExercisesPage = exercisesPage;
            MusclesPage = musclesPage;
            TrainingLogPage = trainingLogPage;
            ProgressPage = progressPage;
            UserPreferencesPage = userPreferencesPage;
            CategoryPage = categoryPage;
        }
    }
}