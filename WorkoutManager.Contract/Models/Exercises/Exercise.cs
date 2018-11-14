using System.Collections.Generic;
using System.Linq;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Base;

namespace WorkoutManager.Contract.Models.Exercises
{
    public class Exercise : Entity
    {
        public string Name { get; set; }
        
        public ContractionType ContractionType { get; set; }

        public double RelativeBodyweight { get; set; }
        
        private IList<ExercisedMuscle> _muscles = new List<ExercisedMuscle>();
        
        public ExercisedMuscle[] Muscles
        {
            get => _muscles.ToArray();
            set => _muscles = value.ToList();
        }

        public void AddMuscle(ExercisedMuscle muscle) => _muscles.Add(muscle);

        public void RemoveMuscle(ExercisedMuscle muscle) => _muscles.RemoveByReference(muscle);

        public override string ToString() => Name;

        public override IEntity GenericClone() => new Exercise
        {
            Name = Name,
            ContractionType = ContractionType,
            RelativeBodyweight = RelativeBodyweight,
            _muscles = _muscles.Select(x => x.Clone()).ToList(),
            Id = Id,
        };
        
        public Exercise Clone() => GenericClone() as Exercise;  
    }
}