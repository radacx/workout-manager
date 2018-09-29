using System.Collections.Generic;
using System.Linq;
using WorkoutManager.Contract.Models.Sessions;

namespace WorkoutManager.Repository
{
    public class TrainingSessionRepository : Repository<TrainingSession>
    {
        public TrainingSessionRepository(string dbFileName) : base(dbFileName) { }

        public override IEnumerable<TrainingSession> GetAll() => Execute(
            collection => collection.Include(session => session.Exercises)
                .Include($"{nameof(TrainingSession.Exercises)}[0].{nameof(SessionExercise.Sets)}")
                .FindAll()
                .ToList()
        );
    }
}