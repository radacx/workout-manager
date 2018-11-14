using System;
using WorkoutManager.Contract.Models.Sessions.Sets;

namespace WorkoutManager.App.Pages.TrainingLog.Dialogs.ExerciseSetDialog.Isometric
{
    internal class IsometricExerciseSetViewModel : ExerciseSetViewModel
    {
        public TimeSpan Duration { get; set; }
        
        public override ExerciseSet ToModel() => new IsometricExerciseSet
        {
            Duration = Duration,
            Weight = WeightValue,
        };
        
        public static IsometricExerciseSetViewModel FromModel(IsometricExerciseSet model) => new IsometricExerciseSetViewModel
        {
            Duration = model.Duration,
            Weight = $"{model.Weight}",
        };
    }
}