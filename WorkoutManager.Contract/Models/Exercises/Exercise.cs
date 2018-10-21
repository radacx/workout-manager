using System;
using System.Collections.Generic;
using System.Linq;
using WorkoutManager.Contract.Models.Misc;

namespace WorkoutManager.Contract.Models.Exercises
{
    public class Exercise : IEntity, IEquatable<Exercise>
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public ContractionType ContractionType { get; set; }

        public double RelativeBodyweight { get; set; }
        
        private List<ExercisedMuscle> _muscles = new List<ExercisedMuscle>();
        
        public ExercisedMuscle[] Muscles
        {
            get => _muscles.ToArray();
            set => _muscles = value.ToList();
        }
        
        public Exercise()
        {
            ContractionType = ContractionType.Dynamic;
        }

        public void AddMuscle(ExercisedMuscle muscle) => _muscles.Add(muscle);

        public void RemoveMuscle(ExercisedMuscle muscle) => _muscles.RemoveAll(x => ReferenceEquals(x, muscle));

        public bool Equals(Exercise other)
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

        public bool Equals(IEntity other) => other is Exercise exercise && Equals(exercise);

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

            return obj.GetType() == GetType() && Equals((Exercise) obj);
        }

        public override int GetHashCode() => Id;

        public override string ToString() => Name;
    }
}