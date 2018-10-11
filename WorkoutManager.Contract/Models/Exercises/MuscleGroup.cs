using System;
using System.Collections.Generic;
using System.Linq;
using WorkoutManager.Contract.Models.Misc;

namespace WorkoutManager.Contract.Models.Exercises
{
    public class MuscleGroup : IEntity, IEquatable<MuscleGroup>
    {
        private List<MuscleHead> _heads = new List<MuscleHead>();
        
        public int Id { get; set; }
        
        public string Name { get; set; }

        public MuscleHead[] Heads
        {
            get => _heads.ToArray();
            set => _heads = value.ToList();
        }

        public void AddHead(MuscleHead head) => _heads.Add(head);

        public void UpdateHead(MuscleHead head)
        {
            var originalHeadIndex = _heads.IndexOf(head);
            
            _heads.RemoveAt(originalHeadIndex);
            _heads.Insert(originalHeadIndex, head);
        }
        
        public void RemoveHead(MuscleHead head) => _heads.Remove(head);

        public bool Equals(MuscleGroup other)
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

        public bool Equals(IEntity other) => other is MuscleGroup muscle && Equals(muscle);

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

            return obj.GetType() == GetType() && Equals((MuscleGroup) obj);
        }

        public override int GetHashCode() => Id;
    }
}