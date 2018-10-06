using WorkoutManager.App.Pages.Exercises.Models;
using WorkoutManager.Contract.Models.ExerciseSet;
using WorkoutManager.Contract.Models.Sessions;

namespace WorkoutManager.App.Pages.TrainingLog.Models
{
    internal class SessionExerciseViewModel 
    {
        public SessionExercise Exercise { get; set; }
        
        public ObservedCollection<IExerciseSet> Sets { get; }

        public SessionExerciseViewModel(SessionExercise exercise)
        {
            Exercise = exercise;

            Sets = new ObservedCollection<IExerciseSet>(exercise.Sets, exercise.AddSet, exercise.RemoveSet);
        }
    }
}