using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Input;
using PubSub.Core;
using WorkoutManager.App.Events;
using WorkoutManager.App.Pages.Muscles.Dialogs.MuscleDialog;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Structures.Collections.Common;
using WorkoutManager.App.Structures.Dialogs;
using WorkoutManager.App.Structures.ViewModels;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Pages.Muscles
{
    internal class MusclesPageViewModel : ViewModelBase
    {
        public static string DialogsIdentifier => "MusclesPageDialogs";

        private readonly Repository<Muscle> _muscleRepository;
        private readonly DialogFactory _dialogs;
        private readonly Hub _hub;
        
        #region Commands

        public ICommand OpenCreateMuscleDialogCommand { get; }
        
        public ICommand OpenEditMuscleDialogCommand { get; }
        
        public ICommand DeleteCommand { get; }

        #endregion


        #region UI Properties

        public ObservableRangeCollection<Muscle> Muscles { get; } = new WpfObservableRangeCollection<Muscle>();

        #endregion
        

        #region MuscleDialog

        private async void OpenCreateMuscleDialogAsync()
        {
            var muscle = new Muscle();

            var dialog = _dialogs.For<MuscleDialogViewModel>(DialogsIdentifier);
            dialog.Data.DialogTitle = "New muscle";
            dialog.Data.SubmitButtonTitle = "Create";
            dialog.Data.Muscle = MuscleViewModel.FromModel(muscle);

            var dialogResult = await dialog.Show();

            if (dialogResult != DialogResult.Ok)
            {
                return;
            }

            muscle = dialog.Data.Muscle.ToModel();
            Muscles.Add(muscle);
            _muscleRepository.Create(muscle);
        }

        private async void OpenEditMuscleDialogAsync(Muscle muscle)
        {
            var muscleClone = muscle.Clone();

            var dialog = _dialogs.For<MuscleDialogViewModel>(DialogsIdentifier);
            dialog.Data.DialogTitle = "Modified muscle";
            dialog.Data.SubmitButtonTitle = "Save";
            dialog.Data.Muscle = MuscleViewModel.FromModel(muscleClone);

            var dialogResult = await dialog.Show();

            if (dialogResult != DialogResult.Ok)
            {
                return;
            }

            muscleClone = dialog.Data.Muscle.ToModel();
            Muscles.Replace(muscle, muscleClone);
            _muscleRepository.Update(muscleClone);
        }
        
        #endregion
        
        private void LoadMuscles() => Muscles.AddRange(_muscleRepository.GetAll());

        private void DeleteMuscle(Muscle muscle)
        {
            Muscles.Remove(muscle);
                    
            Task.Run(() => _muscleRepository.Delete(muscle));
        }
        
        public MusclesPageViewModel(Repository<Muscle> muscleRepository, DialogFactory dialogs, Hub hub)
        {
            _muscleRepository = muscleRepository;
            _dialogs = dialogs;
            _hub = hub;

            OpenCreateMuscleDialogCommand = new Command(OpenCreateMuscleDialogAsync);
            OpenEditMuscleDialogCommand = new Command<Muscle>(OpenEditMuscleDialogAsync);
            DeleteCommand = new Command<Muscle>(DeleteMuscle);

            Muscles.CollectionChanged += MusclesOnCollectionChanged;
            
            Task.Run(LoadMuscles);
        }

        private void MusclesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _hub.Publish(new MusclesChangedEvent(Muscles));
        }
    }
}