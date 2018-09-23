using System;
using System.Collections.Generic;
using WorkoutManager.Contract.Models.Misc;

namespace WorkoutManager.Contract.Models.Sessions
{
    public class TrainingSession : IEntity, IEquatable<TrainingSession>
    {
        public int Id { get; set; }
        
        public DateTime Date { get; set; }
        
        public ICollection<SessionExercise> Exercises { get; } = new List<SessionExercise>();

        public TrainingSession(DateTime date)
        {
            Date = date;
        }

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

            return obj.GetType() == this.GetType() && Equals((TrainingSession) obj);
        }

        public override int GetHashCode() => Id;
    }
}