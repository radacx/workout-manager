using System;
using WorkoutManager.Contract.Models.Misc;

namespace WorkoutManager.Contract.Models.Progress
{
    public class ProgressFilter : IEntity, IEquatable<ProgressFilter>
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public FilterBy? FilterBy { get; set; }
        
        public IEntity FilteringValue { get; set; }
        
        public GroupBy? GroupBy { get; set; }
        
        public FilterMetric? Metric { get; set; }
        
        public bool Equals(ProgressFilter other)
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

        public bool Equals(IEntity other) => other is ProgressFilter filter && Equals(filter);

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

            return obj.GetType() == GetType() && Equals((ProgressFilter) obj);
        }

        public override int GetHashCode() => Id;
    }
}