using System.Collections.Generic;
using System.Linq;
using LiteDB;
using WorkoutManager.Contract;
using WorkoutManager.Contract.Models.Sessions;

namespace WorkoutManager.Repository.Repositories
{
    public class SessionExerciseRepository : Repository<SessionExercise>
    {
        public SessionExerciseRepository(DatabaseConfiguration configuration) : base(configuration) { }

        public static void Register(BsonMapper mapper)
        {
            mapper.Entity<SessionExercise>().DbRef(x => x.Sets).DbRef(x => x.Exercise);
        }

        public override IEnumerable<SessionExercise> GetAll()
            => Execute(collection => collection.Include(x => x.Sets).Include(x => x.Exercise).FindAll().ToList());
    }
}