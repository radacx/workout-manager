using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutManager.App.Misc;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Exercises.Models
{
    public class MotionsPageViewModel
    {
        private readonly Repository<JointMotion> _motionRepository;

        public BulkObservableCollection<JointMotion> Motions { get; } = new BulkObservableCollection<JointMotion>();

        public ICommand Delete { get; }

        public ICommand Create { get; }
        
        private void LoadMotions() => Motions.AddRange(_motionRepository.GetAll());

        private void DeleteMotion(JointMotion motion)
        {
            Motions.Remove(motion);
            
            Task.Run(() => _motionRepository.Delete(motion));
        }

        private void CreateMotion(string name)
        {
            var motion = new JointMotion(name);
            
            Motions.Add(motion);
            
            Task.Run(() => _motionRepository.Create(motion));
        }

        public MotionsPageViewModel(Repository<JointMotion> motionRepository)
        {
            _motionRepository = motionRepository;

            Delete = new Command<JointMotion>(DeleteMotion);
            
            Create = new Command<string>(CreateMotion);
            
            Task.Run(() => LoadMotions());
        }
    }
}