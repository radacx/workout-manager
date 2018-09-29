using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutManager.App.Pages.Motions.Dialogs;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Pages.Motions.Models
{
    internal class MotionsPageViewModel
    {
        private readonly Repository<JointMotion> _motionRepository;
        private readonly DialogViewer<JointMotionDialog> _motionDialogViewer;
        
        public BulkObservableCollection<JointMotion> Motions { get; } = new BulkObservableCollection<JointMotion>();

        public ICommand Delete { get; }

        public ICommand OpenEditMotionDialog { get; }
        
        public ICommand OpenAddMotionDialog { get; }
        
        private void LoadMotions() => Motions.AddRange(_motionRepository.GetAll());

        private void DeleteMotion(JointMotion motion)
        {
            Motions.Remove(motion);
            
            Task.Run(() => _motionRepository.Delete(motion));
        }

        private void CreateMotion(JointMotion motion) => _motionRepository.Create(motion);

        private void UpdateMotion(JointMotion motion) =>_motionRepository.Update(motion);
        
        public MotionsPageViewModel(Repository<JointMotion> motionRepository, DialogViewer<JointMotionDialog> motionDialogViewer)
        {
            _motionRepository = motionRepository;
            _motionDialogViewer = motionDialogViewer;
            
            Delete = new Command<JointMotion>(DeleteMotion);
            
            OpenAddMotionDialog = new Command(
                () =>
                {
                    var motion = new JointMotion();

                    var viewModel = new JointMotionDialogViewModel(motion)
                    {
                        SaveButtonTitle = "Create"
                    };

                    var dialogResult = _motionDialogViewer.WithContext(viewModel).Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    Motions.Add(motion);

                    Task.Run(() => CreateMotion(motion));
                });
            
            OpenEditMotionDialog = new Command<JointMotion>(
                motion =>
                {
                    var motionClone = motion.Clone();

                    var viewModel = new JointMotionDialogViewModel(motionClone)
                    {
                        SaveButtonTitle = "Save"
                    };

                    var dialogResult = _motionDialogViewer.WithContext(viewModel).Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    Motions.Replace(originalMotion => originalMotion.Equals(motionClone), motionClone);

                    Task.Run(() => UpdateMotion(motionClone));
                });
            
            Task.Run(() => LoadMotions());
        }
    }
}