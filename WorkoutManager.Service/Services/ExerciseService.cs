using System.Linq;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;

namespace WorkoutManager.Service.Services
{
    public class ExerciseService : Service<Exercise>
    {
        private readonly Repository<Muscle> _muscleRepository;
        
        public void Create(Exercise exercise)
        {
            _muscleRepository.CreateRange(exercise.PrimaryMuscles);
            _muscleRepository.CreateRange(exercise.SecondaryMuscles);
            
            Repository.Create(exercise);
        }

        public void Update(Exercise exercise)
        {
            var newPrimaryMuscles = exercise.PrimaryMuscles.Where(muscle => muscle.Id == 0);
            var newSecondaryMuscle = exercise.SecondaryMuscles.Where(muscle => muscle.Id == 0);
            var oldPrimaryMuscles = exercise.PrimaryMuscles.Where(muscle => muscle.Id != 0);
            var oldSecondaryMuscles = exercise.SecondaryMuscles.Where(muscle => muscle.Id != 0);
            
            
            _muscleRepository.CreateRange(newPrimaryMuscles);
            _muscleRepository.CreateRange(newSecondaryMuscle);
            
            _muscleRepository.UpdateRange(oldPrimaryMuscles);
            _muscleRepository.UpdateRange(oldSecondaryMuscles);
            
            Repository.Update(exercise);
        }
        
        public void Delete(Exercise exercise)
        {
            _muscleRepository.DeleteRange(exercise.PrimaryMuscles);
            _muscleRepository.DeleteRange(exercise.SecondaryMuscles);
            
            Repository.Delete(exercise);    
        }

        public ExerciseService(Repository<Exercise> repository, Repository<Muscle> muscleRepository) : base(repository)
        {
            _muscleRepository = muscleRepository;
        }
    }
}