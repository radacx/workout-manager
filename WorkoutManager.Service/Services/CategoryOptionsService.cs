using WorkoutManager.Contract.Models.Categories;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;

namespace WorkoutManager.Service.Services
{
    public class CategoryOptionsService
    {
        private readonly Repository<Exercise> _exerciseRepository;
        private readonly Repository<Muscle> _muscleRepository;

        public CategoryOptionsService(Repository<Muscle> muscleRepository, Repository<Exercise> exerciseRepository)
        {
            _muscleRepository = muscleRepository;
            _exerciseRepository = exerciseRepository;
        }

        public CategoryOptions GetOptions()
        {
            var muscles = _muscleRepository.GetAll();
            var exercises = _exerciseRepository.GetAll();

            return new CategoryOptions(muscles, exercises);
        }
    }
}