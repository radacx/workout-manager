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
            _exercisedMuscleRepository.CreateRange(exercise.Muscles);
            
            Repository.Create(exercise);
        }

        public void Update(Exercise exercise)
        {
            var newMuscles = exercise.Muscles.Where(muscle => muscle.Id == 0);
            var oldMuscles = exercise.Muscles.Where(muscle => muscle.Id != 0);
            
            _exercisedMuscleRepository.CreateRange(newMuscles);
            
            _exercisedMuscleRepository.UpdateRange(oldMuscles);
            
            Repository.Update(exercise);
        }
        
        public void Delete(Exercise exercise)
        {
            _exercisedMuscleRepository.DeleteRange(exercise.Muscles);
            
            Repository.Delete(exercise);    
        }

        public ExerciseService(Repository<Exercise> repository, Repository<ExercisedMuscle> exercisedMuscleRepository) : base(repository)
        {
            _exercisedMuscleRepository = exercisedMuscleRepository;
        }
    }
}