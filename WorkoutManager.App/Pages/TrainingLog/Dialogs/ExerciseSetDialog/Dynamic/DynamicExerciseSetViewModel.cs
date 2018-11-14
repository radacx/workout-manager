using WorkoutManager.Contract.Models.Sessions.Sets;

namespace WorkoutManager.App.Pages.TrainingLog.Dialogs.ExerciseSetDialog.Dynamic
{
    internal class DynamicExerciseSetViewModel : ExerciseSetViewModel
    {
        public int Reps { get; set; }

        public override ExerciseSet ToModel() => new Contract.Models.Sessions.Sets.DynamicExerciseSet
        {
            Reps = Reps,
            Weight = WeightValue,
        };
        
        public static DynamicExerciseSetViewModel FromModel(Contract.Models.Sessions.Sets.DynamicExerciseSet model) => new DynamicExerciseSetViewModel
        {
            Reps = model.Reps,
            Weight = $"{model.Weight}",
        };
    }
}