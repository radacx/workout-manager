using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.ExerciseSet;
using WorkoutManager.Contract.Models.Misc;

namespace WorkoutManager.Contract.Models.Sessions
{
    public class SessionExercise : IEntity, IEquatable<SessionExercise>
    {
        public int Id { get; set; }
        
        private ObservableCollection<IExerciseSet> _sets = new ObservableCollection<IExerciseSet>();
        
        public IEnumerable<IExerciseSet> Sets
        {
            get => _sets;
            set => _sets = new ObservableCollection<IExerciseSet>(value);
        }

        public SessionExercise() {}

        public SessionExercise(Exercise exercise)
        {
            Exercise = exercise;
        }

        public Exercise Exercise { get; set; }

        public void AddSet(IExerciseSet set) => _sets.Add(set);

        public void RemoveSet(IExerciseSet set) => _sets.Remove(set);
        
        public bool Equals(SessionExercise other)
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

        public bool Equals(IEntity other) => other is SessionExercise exercise && Equals(exercise);

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

            return obj.GetType() == GetType() && Equals((SessionExercise) obj);
        }

        public override int GetHashCode() => Id;
    }
}