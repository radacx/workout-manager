using WorkoutManager.Contract.Models.Base;

namespace WorkoutManager.Contract.Models.Exercises
{
    public class Muscle : Entity
    {    
        public string Name { get; set; }

        public override string ToString() => Name;

        public override IEntity GenericClone() => new Muscle
        {
            Name = Name,
            Id = Id,
        };
        
        public Muscle Clone() => GenericClone() as Muscle;
    }
}