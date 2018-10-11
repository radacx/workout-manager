using System.Windows.Input;
using Force.DeepCloner;
using WorkoutManager.App.Pages.MuscleGroups.Dialogs;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.Pages.MuscleGroups.Models
{
    internal class MuscleGroupDialogViewModel : ViewModelBase
    {
        private MuscleGroup _muscleGroup;
        public MuscleGroup MuscleGroup
        {
            get => _muscleGroup;
            set => SetField(ref _muscleGroup, value);
        }

        public ObservedCollection<MuscleHead> Heads { get; set; }
        
        public string SaveButtonTitle { get; set; }
        
        public ICommand OpenAddMuscleHeadDialog { get; }
        
        public ICommand OpenEditMuscleHeadDialog { get; }

        public ICommand RemoveHead { get; }
        
        public MuscleGroupDialogViewModel(DialogFactory<MuscleHeadDialog, MuscleHeadDialogViewModel> muscleHeadDialogFactory)
        {
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(MuscleGroup))
                {
                    Heads = new ObservedCollection<MuscleHead>(
                        MuscleGroup.Heads,
                        MuscleGroup.AddHead,
                        MuscleGroup.RemoveHead
                    );
                }  
            };
            
            OpenAddMuscleHeadDialog = new Command(
                () =>
                {
                    var muscleHead = new MuscleHead();

                    var dialog = muscleHeadDialogFactory.Get();
                    dialog.Data.MuscleHead = muscleHead;
                    dialog.Data.SaveButtonTitle = "Add";

                    var dialogResult = dialog.Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }
                    
                    Heads.Add(muscleHead);
                }
            );

            OpenEditMuscleHeadDialog = new Command<MuscleHead>(
                muscleHead =>
                {
                    var muscleHeadClone = muscleHead.DeepClone();
                    
                    var dialog = muscleHeadDialogFactory.Get();
                    dialog.Data.MuscleHead = muscleHeadClone;
                    dialog.Data.SaveButtonTitle = "Save";

                    var dialogResult = dialog.Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }
                    
                    Heads.Replace(muscleHead, muscleHeadClone);
                });
            
            RemoveHead = new Command<MuscleHead>(
                head => Heads.Remove(head)
            );
        }
    }
}