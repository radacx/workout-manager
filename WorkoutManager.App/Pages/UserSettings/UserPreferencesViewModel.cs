using WorkoutManager.App.Structures.ViewModels;
using WorkoutManager.Contract.Models.User;

namespace WorkoutManager.App.Pages.UserSettings
{
    internal class UserPreferencesViewModel : ViewModelBase
    {
        private readonly int _id;
        
        private WeightUnits _weightUnits;

        public WeightUnits WeightUnits
        {
            get => _weightUnits;
            set => SetField(ref _weightUnits, value);
        }

        private UserPreferencesViewModel(int id)
        {    
            _id = id;
        }

        public static UserPreferencesViewModel FromModel(UserPreferences model)
            => new UserPreferencesViewModel(model.Id)
            {
                WeightUnits = model.WeightUnits,
            };

        public UserPreferences ToModel() => new UserPreferences
        {
            Id = _id,
            WeightUnits = WeightUnits,
        };
    }
}