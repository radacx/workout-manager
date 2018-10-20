using System.Collections.Generic;
using System.Linq;
using LiteDB;
using WorkoutManager.Contract;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.Repository.Repositories
{
    public class ExerciseRepository : Repository<Exercise>
    {
        public ExerciseRepository(DatabaseConfiguration configuration) : base(configuration) { }

        public static void Register(BsonMapper mapper)
        {
            mapper.Entity<Exercise>()
                .DbRef(x => x.PrimaryMuscles)
                .DbRef(x => x.SecondaryMuscles);
        }
        
        public override IEnumerable<Exercise> GetAll() => Execute(
            collection => collection
                .Include(x => x.PrimaryMuscles)
                .Include(x => x.PrimaryMuscles[0].Muscle)
                .Include(x => x.SecondaryMuscles)
                .Include(x => x.SecondaryMuscles[0].Muscle)
                .FindAll()
                .ToList()
        );
    }
}