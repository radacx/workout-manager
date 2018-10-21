using System.Collections.Generic;
using WorkoutManager.App.Structures;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.Pages.Exercises.Models
{
    internal class AddExercisedMuscleDialogViewModel : DialogModelBase
    {
        private Muscle _selectedMuscle;
        
        public Muscle SelectedMuscle
        {
            get => _selectedMuscle;
            set => SetField(ref _selectedMuscle, value);
        }

        public int RelativeEngagement { get; set; }

        public IEnumerable<Muscle> Muscles { get; set; }
    }
}