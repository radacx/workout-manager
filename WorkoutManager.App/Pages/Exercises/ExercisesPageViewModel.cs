using System.Threading.Tasks;
using System.Windows.Input;
using Force.DeepCloner;
using WorkoutManager.App.Pages.Exercises.Models;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils.Dialogs;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Service.Services;

namespace WorkoutManager.App.Pages.Exercises
{
    internal class ExercisesPageViewModel : ViewModelBase
    {
        public string ExerciseDialogIdentifier => "ExerciseDialog";

        public ObservableRangeCollection<Exercise> Exercises { get; } = new WpfObservableRangeCollection<Exercise>();
        
        public ICommand OpenCreateExerciseModalDialog { get; }
        
        public ICommand OpenEditExerciseModalDialog { get; }
        
        public ICommand Delete { get; }
        
        private void LoadExercises() => Exercises.AddRange(_exerciseService.GetAll());

        private readonly ExerciseService _exerciseService;
        
        public ExercisesPageViewModel(ExerciseService exerciseService, DialogViewer dialogViewer)
        {
            _exerciseService = exerciseService;

            Exercises.ShapeView().OrderBy(exercise => exercise.Name).Apply();
            
            OpenCreateExerciseModalDialog = new Command(
                async () =>
                {
                    var exercise = new Exercise();

                    var dialog = dialogViewer.For<ExerciseDialogModelViewModel>(ExerciseDialogIdentifier);
                    dialog.Data.Exercise = exercise;
                    dialog.Data.SubmitButtonTitle = "Create";
                    dialog.Data.DialogTitle = "New exercise";

                    var dialogResult = await dialog.Show();
                    
                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }
                    
                    Exercises.Add(exercise);
                    _exerciseService.Create(exercise);
                }
            );

            Delete = new Command<Exercise>(
                exercise =>
                {
                    Exercises.Remove(exercise);

                    Task.Run(() => _exerciseService.Delete(exercise));
                });

            OpenEditExerciseModalDialog = new Command<Exercise>(
                async exercise =>
                {
                    var exerciseClone = exercise.DeepClone();

                    var dialog = dialogViewer.For<ExerciseDialogModelViewModel>(ExerciseDialogIdentifier);
                    dialog.Data.Exercise = exerciseClone;
                    dialog.Data.SubmitButtonTitle = "Save";
                    dialog.Data.DialogTitle = "Modified exercise";
                    
                    var dialogResult = await dialog.Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }
                    
                    Exercises.Replace(exercise, exerciseClone);
                    _exerciseService.Update(exerciseClone);
                });

            Task.Run(LoadExercises);
        }
    }
}
