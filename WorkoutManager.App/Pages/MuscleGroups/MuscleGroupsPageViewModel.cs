using System.Threading.Tasks;
using System.Windows.Input;
using Force.DeepCloner;
using PubSub.Core;
using WorkoutManager.App.Events;
using WorkoutManager.App.Pages.MuscleGroups.Dialogs;
using WorkoutManager.App.Pages.MuscleGroups.Models;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Service.Services;

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
        
        public MuscleGroupsPageViewModel(MuscleGroupService muscleGroupService, DialogFactory<MuscleGroupDialog, MuscleGroupDialogViewModel> muscleGroupDialogFactory, Hub eventAggregator)
        {
            _muscleGroupService = muscleGroupService;
            
            MuscleGroups.ShapeView().OrderBy(muscleGroup => muscleGroup.Name).Apply();
            
            OpenCreateMuscleGroupDialog = new Command(
                () =>
                {
                    var muscleGroup = new MuscleGroup();

                    var dialog = muscleGroupDialogFactory.Get();
                    dialog.Data.MuscleGroup = muscleGroup;
                    dialog.Data.SaveButtonTitle = "Create";

                    var dialogResult = dialog.Show();

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
                    var muscleGroupClone = muscleGroup.DeepClone();

                    var dialog = muscleGroupDialogFactory.Get();
                    dialog.Data.MuscleGroup = muscleGroupClone;
                    dialog.Data.SaveButtonTitle = "Save";

                    var dialogResult = dialog.Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    MuscleGroups.Replace(muscleGroup, muscleGroupClone);
                    eventAggregator.Publish(new MuscleGroupChangedEvent(muscleGroupClone));
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