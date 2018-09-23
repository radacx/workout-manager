using System.Collections.Generic;
using System.Linq;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.Repository
{
    public class MuscleGroupRepository : Repository<MuscleGroup>
    {
        public MuscleGroupRepository(string dbFileName) : base(dbFileName) { }

        public override IEnumerable<MuscleGroup> GetAll() => Execute(
            collection => collection.Include(muscleGroup => muscleGroup.Heads).FindAll().ToList()
        );
    }
}