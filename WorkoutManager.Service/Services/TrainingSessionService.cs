using System.IO;
using System.Linq;
using System.Text;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Repository;

namespace WorkoutManager.Service.Services
{
    public class TrainingSessionService : Service<TrainingSession>
    {
        private readonly Repository<SessionExercise> _sessionExerciseRepository;
        private readonly UserPreferencesService _userPreferencesService;
        
        public void ExportToFile(string fileName)
        {
            var weightUnits = _userPreferencesService.Load().WeightUnits.GetDescription();
            
            var sb = new StringBuilder(512);
            
            var sessions = GetAll().OrderByDescending(session => session.Date).ToArray();
            var count = sessions.Length;
            
            for (var i = 0; i < count; i++)
            {
                var session = sessions[i];
                
                sb.AppendLine($"Date: {session.Date.ToShortDateString()}");
                sb.AppendLine($"Bodyweight: {session.Bodyweight} {weightUnits}");
                sb.AppendLine();

                foreach (var exercise in session.Exercises)
                {
                    sb.AppendLine($"{exercise.Exercise.Name}");

                    foreach (var set in exercise.Sets)
                    {
                        sb.AppendLine($"{set} {weightUnits}");
                    }

                    sb.AppendLine();    
                }

                if (i < count + 1)
                {
                    sb.AppendLine();
                }
            }
            
            File.WriteAllText(fileName, sb.ToString());
        }
        
        public TrainingSessionService(Repository<TrainingSession> repository, Repository<SessionExercise> sessionExerciseRepository, UserPreferencesService userPreferencesService) : base(repository)
        {
            _sessionExerciseRepository = sessionExerciseRepository;
            _userPreferencesService = userPreferencesService;
        }
        
        public void Delete(TrainingSession session)
        {
            _sessionExerciseRepository.DeleteRange(session.Exercises);
            
            Repository.Delete(session);
        }

        public void Update(TrainingSession session)
        {
            var newExercises = session.Exercises.Where(exercise => exercise.Id == 0);
            var oldExercises = session.Exercises.Where(exercise => exercise.Id != 0);
            
            _sessionExerciseRepository.CreateRange(newExercises);
            _sessionExerciseRepository.UpdateRange(oldExercises);
            Repository.Update(session);
        }

        public void Create(TrainingSession session)
        {
            var newExercises = session.Exercises;

            _sessionExerciseRepository.CreateRange(newExercises);
            
            Repository.Create(session);
        }
    }
}