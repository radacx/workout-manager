using System;
using System.Collections.Generic;
using System.Linq;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Base;

namespace WorkoutManager.Contract.Models.Sessions
{ 
    public class TrainingSession : Entity
    {
        private IList<SessionExercise> _exercises = new List<SessionExercise>();
        
        public double Bodyweight { get; set; }
        
        public DateTime Date { get; set; }
        
        public SessionExercise[] Exercises
        {
            get => _exercises.ToArray();
            set => _exercises = value.ToList();
        }

        public void AddExercise(SessionExercise exercise) => _exercises.Add(exercise);
        
        public void RemoveExercise(SessionExercise exercise) => _exercises.RemoveByReference(exercise);

        public override IEntity GenericClone() => new TrainingSession
        {
            Id = Id,
            Bodyweight = Bodyweight,
            Date = Date,
            _exercises = _exercises.Select(x => x.Clone()).ToList(),
        };
        
        public TrainingSession Clone() => GenericClone() as TrainingSession;
    }
}