using System;
using System.Collections.Generic;
using WorkoutManager.Contract.Models.Base;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.Contract.Models.Categories
{
    public class CategoryOptions
    {
        public IEnumerable<Muscle> Muscles { private get; set; }
        public IEnumerable<Exercise> Exercises { private get; set; }
        
        public CategoryOptions(IEnumerable<Muscle> muscles, IEnumerable<Exercise> exercises)
        {
            Muscles = muscles;
            Exercises = exercises;
        }

        public IEnumerable<IEntity> GetOptions(Type optionType)
        {
            if (optionType == typeof(Muscle))
            {
                return Muscles;
            }

            if (optionType == typeof(Exercise))
            {
                return Exercises;
            }

            throw new ArgumentException($"Invalid type: {optionType.FullName}");
        }
    }
}