using System.Threading.Tasks;
using System.Windows.Input;
using Force.DeepCloner;
using PubSub.Core;
using WorkoutManager.App.Pages.Muscles.Dialogs;
using WorkoutManager.App.Pages.Muscles.Models;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Pages.Muscles
{
    internal class MusclesPageViewModel : ViewModelBase
    {
        public BulkObservableCollection<Muscle> Muscles { get; } = new BulkObservableCollection<Muscle>();

        public ICommand OpenCreateMuscleDialog { get; }
        
        public ICommand OpenEditMuscleDialog { get; }
        
        public ICommand Delete { get; }
        
        private void LoadMuscles() => Muscles.AddRange(_muscleRepository.GetAll());

        private readonly Repository<Muscle> _muscleRepository;
        
        public MusclesPageViewModel(Repository<Muscle> muscleRepository, DialogFactory<MuscleDialog, MuscleDialogViewModel> muscleDialogFactory, Hub eventAggregator)
        {
            _muscleRepository = muscleRepository;
            
            Muscles.ShapeView().OrderBy(muscle => muscle.Name).Apply();
            
            OpenCreateMuscleDialog = new Command(
                () =>
                {
                    var muscle = new Muscle();

                    var dialog = muscleDialogFactory.Get();
                    dialog.Data.Muscle = muscle;
                    dialog.Data.SaveButtonTitle = "Create";

                    var dialogResult = dialog.Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    Muscles.Add(muscle);

                    Task.Run(() => _muscleRepository.Create(muscle));
                });
            
            OpenEditMuscleDialog = new Command<Muscle>(
                muscle =>
                {
                    var muscleClone = muscle.DeepClone();

                    var dialog = muscleDialogFactory.Get();
                    dialog.Data.Muscle = muscleClone;
                    dialog.Data.SaveButtonTitle = "Save";

                    var dialogResult = dialog.Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    Muscles.Replace(muscle, muscleClone);

                    Task.Run(() => _muscleRepository.Update(muscleClone));
                });
            
            Delete = new Command<Muscle>(
                muscle =>
                {
                    Muscles.Remove(muscle);
                    
                    Task.Run(() => _muscleRepository.Delete(muscle));
                });
            
            Task.Run(() => LoadMuscles());
        }
    }
}