using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutManager.App.Pages.MuscleGroups.Dialogs;
using WorkoutManager.App.Pages.MuscleGroups.Models;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Service;

namespace WorkoutManager.App.Pages.MuscleGroups
{
    internal class MuscleGroupsPageViewModel
    {
        public BulkObservableCollection<MuscleGroup> MuscleGroups { get; } = new BulkObservableCollection<MuscleGroup>();

        public ICommand OpenCreateMuscleGroupDialog { get; }
        
        public ICommand OpenEditMuscleGroupDialog { get; }
        
        public ICommand Delete { get; }
        
        private void LoadMuscleGroups() => MuscleGroups.AddRange(_muscleGroupService.GetAll());

        private readonly MuscleGroupService _muscleGroupService;
        private readonly DialogViewer<MuscleGroupDialog> _muscleGroupDialogViewer;
        
        public MuscleGroupsPageViewModel(MuscleGroupService muscleGroupService, DialogViewer<MuscleGroupDialog> muscleGroupDialogViewer, DialogViewer<MuscleHeadDialog> muscleHeadDialogViewer)
        {
            _muscleGroupService = muscleGroupService;
            _muscleGroupDialogViewer = muscleGroupDialogViewer;
            
            OpenCreateMuscleGroupDialog = new Command(
                () =>
                {
                    var muscleGroup = new MuscleGroup();

                    var viewModel = new MuscleGroupViewModel(muscleGroup, muscleHeadDialogViewer)
                    {
                        SaveButtonTitle = "Create"
                    };

                    var dialogResult = _muscleGroupDialogViewer.WithContext(viewModel).Show();

                    if (dialogResult != DialogResult.Ok)
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

                    var viewModel = new MuscleGroupViewModel(muscleGroupClone, muscleHeadDialogViewer)
                    {
                        SaveButtonTitle = "Save"
                    };

                    var dialogResult = _muscleGroupDialogViewer.WithContext(viewModel).Show();

                    if (dialogResult != DialogResult.Ok)
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