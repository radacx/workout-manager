using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutManager.App.Exercises.UserControls;
using WorkoutManager.App.Misc;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Service;

namespace WorkoutManager.App.Exercises.Models
{
    public class MuscleGroupsPageViewModel
    {
        public BulkObservableCollection<MuscleGroup> MuscleGroups { get; } = new BulkObservableCollection<MuscleGroup>();

        public ICommand OpenCreateMuscleGroupDialog { get; }
        
        public ICommand OpenEditMuscleGroupDialog { get; }
        
        public ICommand Delete { get; }
        
        private void LoadMuscleGroups() => MuscleGroups.AddRange(_muscleGroupService.GetAll());

        private readonly MuscleGroupService _muscleGroupService;
        
        public MuscleGroupsPageViewModel(MuscleGroupService muscleGroupService)
        {
            _muscleGroupService = muscleGroupService;
            
            OpenCreateMuscleGroupDialog = new Command(
                () =>
                {
                    var muscleGroup = new MuscleGroup();

                    var dialog = new MuscleGroupDialog
                    {
                        DataContext = new MuscleGroupViewModel()
                        {
                            MuscleGroup = muscleGroup,
                            SaveButtonTitle = "Create"
                        }
                    };

                    var dialogResult = dialog.ShowDialog();

                    if (!dialogResult.HasValue || !dialogResult.Value)
                    {
                        return;
                    }

                    MuscleGroups.Add(muscleGroup);

                    Task.Run(() => _muscleGroupService.Create(muscleGroup));
                });
            
            OpenEditMuscleGroupDialog = new Command<MuscleGroup>(
                muscleGroup =>
                {
                    var muscleGroupClone = muscleGroup.Clone();

                    var dialog = new MuscleGroupDialog
                    {
                        DataContext = new MuscleGroupViewModel()
                        {
                            MuscleGroup = muscleGroupClone,
                            SaveButtonTitle = "Save"
                        }
                    };

                    var dialogResult = dialog.ShowDialog();

                    if (!dialogResult.HasValue || !dialogResult.Value)
                    {
                        return;
                    }

                    MuscleGroups.Replace(originalMuscleGroup => Equals(originalMuscleGroup, muscleGroupClone), muscleGroupClone);

                    Task.Run(() => _muscleGroupService.Update(muscleGroupClone));
                });
            
            Delete = new Command<MuscleGroup>(
                muscleGroup =>
                {
                    MuscleGroups.Remove(muscleGroup);
                    
                    Task.Run(() => _muscleGroupService.Delete(muscleGroup));
                });
            
            Task.Run(() => LoadMuscleGroups());
        }
    }
}