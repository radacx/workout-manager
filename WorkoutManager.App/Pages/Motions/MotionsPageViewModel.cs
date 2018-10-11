using System.Threading.Tasks;
using System.Windows.Input;
using Force.DeepCloner;
using WorkoutManager.App.Pages.Motions.Dialogs;
using WorkoutManager.App.Pages.Motions.Models;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Pages.Motions
{
    internal class MotionsPageViewModel
    {
        private readonly Repository<JointMotion> _motionRepository;
        
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
        
        public MotionsPageViewModel(Repository<JointMotion> motionRepository, DialogFactory<JointMotionDialog, JointMotionDialogViewModel> motionDialogFactory)
        {
            _motionRepository = motionRepository;
            
            Delete = new Command<JointMotion>(DeleteMotion);
            
            OpenAddMotionDialog = new Command(
                () =>
                {
                    var motion = new JointMotion();

                    var dialog = motionDialogFactory.Get();
                    dialog.Data.Motion = motion;
                    dialog.Data.SaveButtonTitle = "Create";

                    var dialogResult = dialog.Show();

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
                    var motionClone = motion.DeepClone();

                    var dialog = motionDialogFactory.Get();
                    dialog.Data.Motion = motionClone;
                    dialog.Data.SaveButtonTitle = "Save";

                    var dialogResult = dialog.Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    Motions.Replace(motion, motionClone);

                    Task.Run(() => UpdateMotion(motionClone));
                });
            
            Task.Run(() => LoadMotions());
        }
    }
}