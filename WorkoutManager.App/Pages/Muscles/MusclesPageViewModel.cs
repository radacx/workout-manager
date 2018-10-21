using System.Threading.Tasks;
using System.Windows.Input;
using Force.DeepCloner;
using WorkoutManager.App.Pages.Muscles.Models;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils.Dialogs;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Pages.Muscles
{
    internal class MusclesPageViewModel : ViewModelBase
    {
        public string MuscleDialogIdentifier => "MuscleDialog";
        
        public ObservableRangeCollection<Muscle> Muscles { get; } = new WpfObservableRangeCollection<Muscle>();

        public ICommand OpenCreateMuscleDialog { get; }
        
        public ICommand OpenEditMuscleDialog { get; }
        
        public ICommand Delete { get; }
        
        private void LoadMuscles() => Muscles.AddRange(_muscleRepository.GetAll());

        private readonly Repository<Muscle> _muscleRepository;
        
        public MusclesPageViewModel(Repository<Muscle> muscleRepository, DialogViewer dialogViewer)
        {
            _muscleRepository = muscleRepository;
            
            Muscles.ShapeView().OrderBy(muscle => muscle.Name).Apply();
            
            OpenCreateMuscleDialog = new Command(
               async () =>
                {
                    var muscle = new Muscle();

                    var dialog = dialogViewer.For<MuscleDialogViewModel>(MuscleDialogIdentifier);
                    dialog.Data.Muscle = muscle;
                    dialog.Data.DialogTitle = "New muscle";
                    dialog.Data.SubmitButtonTitle = "Submit";

                    var dialogResult = await dialog.Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    Muscles.Add(muscle);

                    _muscleRepository.Create(muscle);
                });
            
            OpenEditMuscleDialog = new Command<Muscle>(
                async muscle =>
                {
                    var muscleClone = muscle.DeepClone();

                    var dialog = dialogViewer.For<MuscleDialogViewModel>(MuscleDialogIdentifier);
                    dialog.Data.Muscle = muscleClone;
                    dialog.Data.SubmitButtonTitle = "Save";
                    dialog.Data.DialogTitle = "Modified muscle";

                    var dialogResult = await dialog.Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    Muscles.Replace(muscle, muscleClone);

                    _muscleRepository.Update(muscleClone);
                });
            
            Delete = new Command<Muscle>(
                muscle =>
                {
                    Muscles.Remove(muscle);
                    
                    Task.Run(() => _muscleRepository.Delete(muscle));
                });
            
            Task.Run(LoadMuscles);
        }
    }
}