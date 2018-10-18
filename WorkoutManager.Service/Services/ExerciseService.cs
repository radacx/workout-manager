using System.Linq;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;

namespace WorkoutManager.Service.Services
{
    public class ExerciseService : Service<Exercise>
    {
        private readonly Repository<MuscleGroup> _muscleGroupRepository;
        
        public void Create(Exercise exercise)
        {
            _muscleGroupRepository.CreateRange(exercise.PrimaryMuscles);
            _muscleGroupRepository.CreateRange(exercise.SecondaryMuscles);
            
            Repository.Create(exercise);
        }

        public void Update(Exercise exercise)
        {
            var newPrimaryMuscles = exercise.PrimaryMuscles.Where(muscle => muscle.Id == 0);
            var newSecondaryMuscle = exercise.SecondaryMuscles.Where(muscle => muscle.Id == 0);
            var oldPrimaryMuscles = exercise.PrimaryMuscles.Where(muscle => muscle.Id != 0);
            var oldSecondaryMuscles = exercise.SecondaryMuscles.Where(muscle => muscle.Id != 0);
            
            
            _muscleGroupRepository.CreateRange(newPrimaryMuscles);
            _muscleGroupRepository.CreateRange(newSecondaryMuscle);
            
            _muscleGroupRepository.UpdateRange(oldPrimaryMuscles);
            _muscleGroupRepository.UpdateRange(oldSecondaryMuscles);
            
            Repository.Update(exercise);
        }
        
        public void Delete(Exercise exercise)
        {
            _muscleGroupRepository.DeleteRange(exercise.PrimaryMuscles);
            _muscleGroupRepository.DeleteRange(exercise.SecondaryMuscles);
            
            Repository.Delete(exercise);    
        }

        public ExerciseService(Repository<Exercise> repository, Repository<MuscleGroup> muscleGroupRepository) : base(repository)
        {
            _muscleGroupRepository = muscleGroupRepository;
        }
    }
}