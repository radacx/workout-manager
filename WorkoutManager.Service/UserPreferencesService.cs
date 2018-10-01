using System.Linq;
using WorkoutManager.Contract.Models.User;
using WorkoutManager.Repository;

namespace WorkoutManager.Service
{
    public class UserPreferencesService
    {
        private readonly Repository<UserPreferences> _userPreferencesRepository;

        private UserPreferences _userPreferences;
        
        public UserPreferencesService(Repository<UserPreferences> userPreferencesRepository)
        {
            _userPreferencesRepository = userPreferencesRepository;
            _userPreferences = _userPreferencesRepository.GetAll().FirstOrDefault();

            if (_userPreferences != null)
            {
                return;
            }

            _userPreferences = new UserPreferences
            {
                WeightUnits = WeightUnits.Kilograms
            };
            
            _userPreferencesRepository.Create(_userPreferences);
        }

        public UserPreferences Load() => _userPreferences;

        public void Update(UserPreferences userPreferences)
        {
            _userPreferences = userPreferences;
            _userPreferencesRepository.Update(userPreferences);
        }
    }
}