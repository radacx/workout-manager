using System.Collections.Generic;
using System.Linq;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Sessions.Sets;

namespace WorkoutManager.Contract.Models.Sessions
{
    public class SessionExercise
    {
        private IList<ExerciseSet> _sets = new List<ExerciseSet>();
        
        public ExerciseSet[] Sets
        {
            get => _sets.ToArray();
            set => _sets = value.ToList();
        }

        public Exercise Exercise { get; set; }

        public void AddSet(ExerciseSet set) => _sets.Add(set);

        public void RemoveSet(ExerciseSet set) => _sets.RemoveByReference(set);

        public override string ToString() => Exercise.Name;

        public SessionExercise Clone() => new SessionExercise
        {
            Exercise = Exercise.Clone(),
            _sets = _sets.Select(x => x.Clone()).ToList(),
        };
    }
}