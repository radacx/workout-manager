using System;
using System.Collections.Generic;
using System.Linq;
using WorkoutManager.Contract.Models.Misc;

namespace WorkoutManager.Contract.Models.Exercises
{
    public class ExercisedMuscle : IEntity, IEquatable<ExercisedMuscle>
    {
        private List<MuscleHead> _usedHeads = new List<MuscleHead>();
        
        public int Id { get; set; }
        
        public MuscleGroup MuscleGroup { get; set; }

        public IEnumerable<MuscleHead> UsedHeads
        {
            get => _usedHeads;
            set => _usedHeads = value.ToList();
        }

        public ExercisedMuscle() { }

        public ExercisedMuscle(MuscleGroup muscleGroup)
        {
            MuscleGroup = muscleGroup;
        }

        public bool Equals(ExercisedMuscle other)
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

        public bool Equals(IEntity other) => other is ExercisedMuscle muscle && Equals(muscle);

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

            return obj.GetType() == this.GetType() && Equals((ExercisedMuscle) obj);
        }

        public override int GetHashCode() => Id;
    }
}