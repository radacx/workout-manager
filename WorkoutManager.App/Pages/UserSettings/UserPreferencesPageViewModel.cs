using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Structures.ViewModels;
using WorkoutManager.Service.Services;

namespace WorkoutManager.App.Pages.UserSettings
{
    internal class UserPreferencesPageViewModel : ViewModelBase
    {
        public UserPreferencesViewModel UserPreferences { get; set; }

        private readonly UserPreferencesService _userPreferencesService;

        public ICommand SavePreferences { get; }
        
        public ICommand ClearDatabase { get; }
        
        public UserPreferencesPageViewModel(UserPreferencesService userPreferencesService, DatabaseService databaseService)
        {
            _userPreferencesService = userPreferencesService;

            ClearDatabase = new Command(databaseService.DropDatabase);

            SavePreferences = new Command(
                () => { _userPreferencesService.Update(UserPreferences.ToModel()); }
            );

            Task.Run(
                () => { UserPreferences = UserPreferencesViewModel.FromModel(_userPreferencesService.Load()); }
            );
        }
    }
}