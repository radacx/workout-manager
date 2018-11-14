using System.Collections.Generic;
using System.Linq;
using WorkoutManager.Contract;
using WorkoutManager.Contract.Models.Sessions;

namespace WorkoutManager.Repository.Repositories
{
    public class TrainingSessionRepository : Repository<TrainingSession>
    {
        public TrainingSessionRepository(DatabaseConfiguration configuration) : base(configuration) { }

        public override IEnumerable<TrainingSession> GetAll() => Execute(
            collection => collection
                .Include(x => x.Exercises[0].Exercise)
                .Include(x => x.Exercises[0].Exercise.Muscles[0].Muscle)
                .FindAll()
                .ToArray()
        );
    }
}