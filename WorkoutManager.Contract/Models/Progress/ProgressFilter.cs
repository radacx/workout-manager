using WorkoutManager.Contract.Models.Base;

namespace WorkoutManager.Contract.Models.Progress
{
    public class ProgressFilter : Entity
    {
        public string Name { get; set; }
        
        public FilterBy? FilterBy { get; set; }
        
        public IEntity FilteringValue { get; set; }
        
        public GroupBy? GroupBy { get; set; }
        
        public FilterMetric? Metric { get; set; }

        public override IEntity GenericClone() => new ProgressFilter
        {
            Id = Id,
            FilterBy = FilterBy,
            Name = Name,
            GroupBy = GroupBy,
            Metric = Metric,
            FilteringValue = FilteringValue?.GenericClone(),
        };
        
        public ProgressFilter Clone() => GenericClone() as ProgressFilter;
    }
}