using System.Linq;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;

namespace WorkoutManager.Service
{
    public class MuscleGroupService : Service<MuscleGroup>
    {
        private readonly Repository<MuscleHead> _muscleHeadRepository;
        
        public void Delete(MuscleGroup muscleGroup)
        {
            _muscleHeadRepository.DeleteRange(muscleGroup.Heads);
            
            Repository.Delete(muscleGroup);
        }

        public void Create(MuscleGroup muscleGroup)
        {
            _muscleHeadRepository.CreateRange(muscleGroup.Heads);
            
            Repository.Create(muscleGroup);
        }

        public void Update(MuscleGroup muscleGroup)
        {
            var newHeads = muscleGroup.Heads.Where(head => head.Id == 0);
            
            _muscleHeadRepository.CreateRange(newHeads);
            
            Repository.Update(muscleGroup);
        }

        public MuscleGroupService(Repository<MuscleGroup> repository, Repository<MuscleHead> muscleHeadRepository) : base(repository)
        {
            _muscleHeadRepository = muscleHeadRepository;
        }
    }
}