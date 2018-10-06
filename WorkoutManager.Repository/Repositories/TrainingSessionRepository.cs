using System.Collections.Generic;
using System.Linq;
using LiteDB;
using WorkoutManager.Contract.Models.Sessions;

namespace WorkoutManager.Repository.Repositories
{
    public class TrainingSessionRepository : Repository<TrainingSession>
    {
        public TrainingSessionRepository(string dbFileName) : base(dbFileName) { }

        public static void Register(BsonMapper mapper)
        {
            mapper.Entity<TrainingSession>().DbRef(x => x.Exercises);
        }
        
        public override IEnumerable<TrainingSession> GetAll() => Execute(
            collection => collection
                .Include(x => x.Exercises)
                .Include(x => x.Exercises[0].Sets)
                .Include(x => x.Exercises[0].Exercise)
                .FindAll()
                .ToList()
        );
    }
}