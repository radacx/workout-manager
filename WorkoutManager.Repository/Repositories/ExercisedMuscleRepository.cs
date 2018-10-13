using System.Collections.Generic;
using System.Linq;
using LiteDB;
using WorkoutManager.Contract;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.Repository.Repositories
{
    public class ExercisedMuscleRepository : Repository<ExercisedMuscle>
    {
        public ExercisedMuscleRepository(DatabaseConfiguration configuration) : base(configuration) { }

        public static void Register(BsonMapper mapper)
        {
            mapper.Entity<ExercisedMuscle>().DbRef(x => x.MuscleGroup).DbRef(x => x.UsedHeads);
        }

        public override IEnumerable<ExercisedMuscle> GetAll() => Execute(
            collection => collection.Include(x => x.MuscleGroup).Include(x => x.UsedHeads).FindAll().ToList()
        );
    }
}