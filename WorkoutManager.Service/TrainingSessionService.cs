using System.Linq;
using WorkoutManager.Contract.Models.ExerciseSet;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Repository;

namespace WorkoutManager.Service
{
    public class TrainingSessionService : Service<TrainingSession>
    {
        private readonly Repository<SessionExercise> _sessionExerciseRepository;
        private readonly Repository<IExerciseSet> _exerciseSetRepository;
        
        public TrainingSessionService(Repository<TrainingSession> repository, Repository<SessionExercise> sessionExerciseRepository, Repository<IExerciseSet> exerciseSetRepository) : base(repository)
        {
            _sessionExerciseRepository = sessionExerciseRepository;
            _exerciseSetRepository = exerciseSetRepository;
        }
        
        public void Delete(TrainingSession session)
        {
            _exerciseSetRepository.DeleteRange(session.Exercises.SelectMany(exercise => exercise.Sets));
            _sessionExerciseRepository.DeleteRange(session.Exercises);
            
            Repository.Delete(session);
        }

        public void Update(TrainingSession session)
        {
            var newSets = session.Exercises.SelectMany(exercise => exercise.Sets).Where(set => set.Id == 0);
            var oldSets = session.Exercises.SelectMany(exercise => exercise.Sets).Where(set => set.Id != 0);
            var newExercises = session.Exercises.Where(exercise => exercise.Id == 0);
            var oldExercises = session.Exercises.Where(exercise => exercise.Id != 0);
            
            _exerciseSetRepository.CreateRange(newSets);
            _exerciseSetRepository.UpdateRange(oldSets);
            _sessionExerciseRepository.CreateRange(newExercises);
            _sessionExerciseRepository.UpdateRange(oldExercises);
            Repository.Update(session);
        }

        public void Create(TrainingSession session)
        {
            var newSets = session.Exercises.SelectMany(exercise => exercise.Sets);
            var newExercises = session.Exercises;

            _exerciseSetRepository.CreateRange(newSets);
            _sessionExerciseRepository.CreateRange(newExercises);
            
            Repository.Create(session);
        }
    }
}