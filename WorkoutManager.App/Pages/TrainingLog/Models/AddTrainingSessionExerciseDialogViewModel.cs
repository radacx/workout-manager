using System.Collections.Generic;
using WorkoutManager.App.Structures;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.Pages.TrainingLog.Models
{
    internal class AddTrainingSessionExerciseDialogViewModel : DialogModelBase
    {
        private Exercise _selectedExercise;
        
        public IEnumerable<Exercise> Exercises { get; set; }

        public Exercise SelectedExercise
        {
            get => _selectedExercise;
            set => SetField(ref _selectedExercise, value);
        }
    }
}