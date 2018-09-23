using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WorkoutManager.Contract.Annotations;
using WorkoutManager.Contract.Models.Misc;

namespace WorkoutManager.Contract.Models.Exercises
{
    public class MuscleGroup : IEntity, IEquatable<MuscleGroup>
    {
        private ObservableCollection<MuscleHead> _heads = new ObservableCollection<MuscleHead>();
        
        public int Id { get; set; }
        
        public string Name { get; set; }

        public IEnumerable<MuscleHead> Heads
        {
            get => _heads;
            set => _heads = new ObservableCollection<MuscleHead>(value);
        }

        public void AddHead(string name) => _heads.Add(new MuscleHead(name));
        
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

            return obj.GetType() == this.GetType() && Equals((MuscleGroup) obj);
        }

        public override int GetHashCode() => Id;
    }
}