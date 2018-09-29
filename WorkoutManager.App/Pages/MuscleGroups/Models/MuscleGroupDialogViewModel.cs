using System.Windows.Input;
using WorkoutManager.App.Structures;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.Pages.MuscleGroups.Models
{
    internal class MuscleGroupViewModel
    {
        public MuscleGroup MuscleGroup { get; }
        
        public string SaveButtonTitle { get; set; }
        
        public ICommand AddHead { get; }

        public ICommand RemoveHead { get; }
        
        public MuscleGroupViewModel(MuscleGroup muscleGroup)
        {
            MuscleGroup = muscleGroup;

            AddHead = new Command<string>(
                headName => MuscleGroup.AddHead(headName)
            );

            RemoveHead = new Command<MuscleHead>(
                head => MuscleGroup.RemoveHead(head)
            );
        }
    }
}