using System.Collections.Generic;
using System.Linq;
using LiteDB;
using WorkoutManager.Contract;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.Repository.Repositories
{
    public class MuscleGroupRepository : Repository<MuscleGroup>
    {
        public MuscleGroupRepository(DatabaseConfiguration configuration) : base(configuration) { }

        public static void Register(BsonMapper mapper)
        {
            mapper.Entity<MuscleGroup>().DbRef(x => x.Heads);
        }
        
        public override IEnumerable<MuscleGroup> GetAll() => Execute(
            collection => collection.Include(x => x.Heads).FindAll().ToList()
        );
    }
}