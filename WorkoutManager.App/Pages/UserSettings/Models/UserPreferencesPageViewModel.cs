using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutManager.App.Structures;
using WorkoutManager.Contract.Models.User;
using WorkoutManager.Service;

namespace WorkoutManager.App.Pages.UserSettings.Models
{
    internal class UserPreferencesPageViewModel
    {
        public UserPreferences UserPreferences { get; set; }

        private readonly UserPreferencesService _userPreferencesService;

        public ICommand SavePreferences { get; }
        
        public UserPreferencesPageViewModel(UserPreferencesService userPreferencesService)
        {
            _userPreferencesService = userPreferencesService;

            SavePreferences = new Command(
                () => { _userPreferencesService.Update(UserPreferences); });
            
            Task.Run(
                () =>
                {
                    UserPreferences = _userPreferencesService.Load();
                }
            );
        }
    }
}