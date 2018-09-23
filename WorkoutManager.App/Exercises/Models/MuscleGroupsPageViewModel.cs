using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutManager.App.Exercises.UserControls;
using WorkoutManager.App.Misc;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;
    
namespace WorkoutManager.App.Exercises.Models
{
    public class MuscleGroupsPageViewModel
    {
        public BulkObservableCollection<MuscleGroup> MuscleGroups { get; } = new BulkObservableCollection<MuscleGroup>();

        public ICommand OpenCreateMuscleGroupDialog { get; }
        
        public ICommand OpenEditMuscleGroupDialog { get; }
        
        public ICommand Delete { get; }
        
        private void LoadMuscleGroups() => MuscleGroups.AddRange(_muscleGroupRepository.GetAll());

        private void DeleteMuscleGroup(MuscleGroup muscleGroup) => _muscleGroupRepository.Delete(muscleGroup);

        private void CreateMuscleGroup(MuscleGroup muscleGroup)
        {
            _muscleHeadRepository.CreateRange(muscleGroup.Heads);
            _muscleGroupRepository.Create(muscleGroup);
        }

        private void UpdateMuscleGroup(MuscleGroup muscleGroup)
        {
            var newHeads = muscleGroup.Heads.Where(head => head.Id == 0);
            
            _muscleHeadRepository.CreateRange(newHeads);
            _muscleGroupRepository.Update(muscleGroup);
        }

        private readonly Repository<MuscleGroup> _muscleGroupRepository;

        private readonly Repository<MuscleHead> _muscleHeadRepository;
        
        public MuscleGroupsPageViewModel(Repository<MuscleGroup> muscleGroupRepository, Repository<MuscleHead> muscleHeadRepository)
        {
            _muscleGroupRepository = muscleGroupRepository;
            _muscleHeadRepository = muscleHeadRepository;
            
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

                    Task.Run(() => CreateMuscleGroup(muscleGroup));
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

                    Task.Run(() => UpdateMuscleGroup(muscleGroupClone));
                });
            
            Delete = new Command<MuscleGroup>(
                muscleGroup =>
                {
                    MuscleGroups.Remove(muscleGroup);
                    
                    Task.Run(() => DeleteMuscleGroup(muscleGroup));
                });
            
            Task.Run(() => LoadMuscleGroups());
        }
    }
}