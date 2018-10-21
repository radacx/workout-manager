using System;
using System.Collections.Generic;
using System.Linq;
using WorkoutManager.Contract.Models.Misc;

namespace WorkoutManager.Contract.Models.Sessions
{ 
    public class TrainingSession : IEntity, IEquatable<TrainingSession>
    {
        private List<SessionExercise> _exercises = new List<SessionExercise>();
        
        public int Id { get; set; }
        
        public int Bodyweight { get; set; }
        
        public DateTime Date { get; set; }

        public string DateString => Date.ToString("dd.MM.yy ddd");
        
        public SessionExercise[] Exercises
        {
            get => _exercises.ToArray();
            set => _exercises = value.ToList();
        }

        public void AddExercise(SessionExercise exercise) => _exercises.Add(exercise);
        
        public void RemoveExercise(SessionExercise exercise) => _exercises.Remove(exercise);
        
        public bool Equals(TrainingSession other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id == other.Id;
        }

        public bool Equals(IEntity other) => other is TrainingSession session && Equals(session);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((TrainingSession) obj);
        }

        public override int GetHashCode() => Id;
    }
}