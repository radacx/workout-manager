using System.Collections.Generic;
using System.Linq;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.Repository
{
    public class ExerciseRepository : Repository<Exercise>
    {
        public ExerciseRepository(string dbFileName) : base(dbFileName) { }

        public override IEnumerable<Exercise> GetAll() => Execute(
            collection => collection.Include(exercise => exercise.Motions)
                .Include(exercise => exercise.PrimaryMuscles)
                .Include($"{nameof(Exercise.PrimaryMuscles)}.{nameof(ExercisedMuscle.UsedHeads)}")
                .Include(exercise => exercise.SecondaryMuscles)
                .Include($"{nameof(Exercise.SecondaryMuscles)}.{nameof(ExercisedMuscle.UsedHeads)}")
                .FindAll()
                .ToList()
        );
    }
}