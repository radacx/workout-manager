using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutManager.App.Pages.TrainingLog.Dialogs.ExerciseSetDialog;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Structures.Collections.Common;
using WorkoutManager.App.Structures.Dialogs;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Sessions.Sets;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Pages.TrainingLog.Dialogs.SessionExerciseDialog
{
    internal class SessionExerciseDialogViewModel : DialogModelBase
    {
        public static string DialogsIdentifier => "SessionExerciseDialogDialogs";

        private readonly Repository<Exercise> _exerciseRepository;
        private readonly DialogFactory _dialogs;     


        #region Commands

        public ICommand OpenAddExerciseSetDialogCommand { get; }
        
        public ICommand OpenEditExerciseSetDialogCommand { get; }
        
        public ICommand RemoveExerciseSetCommand { get; }

        #endregion


        #region UI Properties

        private SessionExerciseViewModel _sessionExercise;
        public SessionExerciseViewModel SessionExercise
        {
            get => _sessionExercise;
            set => SetField(ref _sessionExercise, value);
        }

        public ObservableRangeCollection<Exercise> Exercises { get; } = new WpfObservableRangeCollection<Exercise>();    
        
        public bool IsDynamicExercise
        {
            get
            {
                if (SessionExercise.Exercise == null)
                {
                    return false;
                }
                
                return SessionExercise.Exercise.ContractionType == ContractionType.Dynamic;
            }
        }

        public bool IsIsometricExercise
        {
            get
            {
                if (SessionExercise.Exercise == null)
                {
                    return false;
                }
                
                return SessionExercise.Exercise.ContractionType == ContractionType.Isometric;
            }
        }

        #endregion


        #region ExerciseSetDialog

        private async void OpenAddExerciseSetDialogAsync()
        {
            var set = SetCreator.Create(SessionExercise.Exercise.ContractionType);

            var dialog = _dialogs.For<ExerciseSetDialogViewModel>(DialogsIdentifier);
            dialog.Data.DialogTitle = "New set";
            dialog.Data.SubmitButtonTitle = "Create";
            dialog.Data.ExerciseSet = ExerciseSetViewModel.FromModel(set);

            var dialogResult = await dialog.Show();

            if (dialogResult != DialogResult.Ok)
            {
                return;
            }

            set = dialog.Data.ExerciseSet.ToModel();
            SessionExercise.Sets.Add(set);
        }
        
        private async void OpenEditExerciseSetDialogAsync(ExerciseSet set)
        {
            var setClone = set.Clone(); 

            var dialog = _dialogs.For<ExerciseSetDialogViewModel>(DialogsIdentifier);
            dialog.Data.DialogTitle = "Modified set";
            dialog.Data.SubmitButtonTitle = "Save";
            dialog.Data.ExerciseSet = ExerciseSetViewModel.FromModel(setClone);

            var dialogResult = await dialog.Show();

            if (dialogResult != DialogResult.Ok)
            {
                return;
            }

            setClone = dialog.Data.ExerciseSet.ToModel();
            SessionExercise.Sets.Replace(set, setClone);
        }

        #endregion
        
        
        private void LoadData()
        {
            Exercises.AddRange(_exerciseRepository.GetAll());
        }

        private void RemoveSet(ExerciseSet set)
        {
            SessionExercise.Sets.RemoveByReference(set);
        }
        
        public SessionExerciseDialogViewModel(Repository<Exercise> exerciseRepository, DialogFactory dialogs)
        {
            _exerciseRepository = exerciseRepository;
            _dialogs = dialogs;

            OpenAddExerciseSetDialogCommand = new Command(OpenAddExerciseSetDialogAsync);
            OpenEditExerciseSetDialogCommand = new Command<ExerciseSet>(OpenEditExerciseSetDialogAsync);
            RemoveExerciseSetCommand = new Command<ExerciseSet>(RemoveSet);

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(SessionExercise))
                {
                    SessionExercise.PropertyChanged += SessionExerciseOnPropertyChanged;    
                }
            };
            
            Task.Run(LoadData);
        }

        private void SessionExerciseOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SessionExercise.Exercise))
            {
                OnPropertyChanged(nameof(IsDynamicExercise));
                OnPropertyChanged(nameof(IsIsometricExercise));
            }
        }
    }
}