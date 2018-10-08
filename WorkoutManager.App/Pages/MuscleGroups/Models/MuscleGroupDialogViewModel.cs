using System.Windows.Input;
using Force.DeepCloner;
using WorkoutManager.App.Pages.Exercises.Models;
using WorkoutManager.App.Pages.MuscleGroups.Dialogs;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.Pages.MuscleGroups.Models
{
    internal class MuscleGroupViewModel
    {
        public MuscleGroup MuscleGroup { get; }
        
        public ObservedCollection<MuscleHead> Heads { get; }
        
        public string SaveButtonTitle { get; set; }
        
        public ICommand OpenAddMuscleHeadDialog { get; }
        
        public ICommand OpenEditMuscleHeadDialog { get; }

        public ICommand RemoveHead { get; }
        
        public MuscleGroupViewModel(MuscleGroup muscleGroup)
        {
            MuscleGroup = muscleGroup;

            Heads = new ObservedCollection<MuscleHead>(muscleGroup.Heads, muscleGroup.AddHead, muscleGroup.RemoveHead);
            
            OpenAddMuscleHeadDialog = new Command(
                () =>
                {
                    var muscleHead = new MuscleHead();
                    var viewModel = new MuscleHeadDialogViewModel(muscleHead)
                    {
                        SaveButtonTitle = "Add"
                    };

                    var dialogResult = DialogBuilder.Create<MuscleHeadDialog>().WithContext(viewModel).Show();

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
                    var viewModel = new MuscleHeadDialogViewModel(muscleHeadClone)
                    {
                        SaveButtonTitle = "Save"
                    };

                    var dialogResult = DialogBuilder.Create<MuscleHeadDialog>().WithContext(viewModel).Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }
                    
                    MuscleGroup.UpdateHead(muscleHeadClone);
                });
            
            RemoveHead = new Command<MuscleHead>(
                head => Heads.Remove(head)
            );
        }
    }
}