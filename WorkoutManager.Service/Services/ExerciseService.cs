using System.Linq;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;

namespace WorkoutManager.Service.Services
{
    public class ExerciseService : Service<Exercise>
    {
        private readonly Repository<ExercisedMuscle> _exercisedMuscleRepository;
        
        public void Create(Exercise exercise)
        {
            _exercisedMuscleRepository.CreateRange(exercise.PrimaryMuscles);
            _exercisedMuscleRepository.CreateRange(exercise.SecondaryMuscles);
            
            Repository.Create(exercise);
        }

        public void Update(Exercise exercise)
        {
            var newPrimaryMuscles = exercise.PrimaryMuscles.Where(muscle => muscle.Id == 0);
            var newSecondaryMuscle = exercise.SecondaryMuscles.Where(muscle => muscle.Id == 0);
            var oldPrimaryMuscles = exercise.PrimaryMuscles.Where(muscle => muscle.Id != 0);
            var oldSecondaryMuscles = exercise.SecondaryMuscles.Where(muscle => muscle.Id != 0);
            
            
            _exercisedMuscleRepository.CreateRange(newPrimaryMuscles);
            _exercisedMuscleRepository.CreateRange(newSecondaryMuscle);
            
            _exercisedMuscleRepository.UpdateRange(oldPrimaryMuscles);
            _exercisedMuscleRepository.UpdateRange(oldSecondaryMuscles);
            
            Repository.Update(exercise);
        }
        
        public void Delete(Exercise exercise)
        {
            _exercisedMuscleRepository.DeleteRange(exercise.PrimaryMuscles);
            _exercisedMuscleRepository.DeleteRange(exercise.SecondaryMuscles);
            
            Repository.Delete(exercise);    
        }

        public ExerciseService(Repository<Exercise> repository, Repository<ExercisedMuscle> exercisedMuscleRepository) : base(repository)
        {
            _exercisedMuscleRepository = exercisedMuscleRepository;
        }
    }
}