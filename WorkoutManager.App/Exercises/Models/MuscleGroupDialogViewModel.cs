using System.Windows.Input;
using WorkoutManager.App.Misc;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.Exercises.Models
{
    internal class MuscleGroupViewModel
    {
        public MuscleGroup MuscleGroup { get; set; }
        
        public string SaveButtonTitle { get; set; }
        
        public ICommand AddHead { get; }

        public ICommand RemoveHead { get; }
        
        public MuscleGroupViewModel()
        {
            AddHead = new Command<string>(
                headName => MuscleGroup.AddHead(headName)
            );

            RemoveHead = new Command<MuscleHead>(
                head => MuscleGroup.RemoveHead(head)
            );
        }
    }
}