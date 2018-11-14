using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WorkoutManager.App.Pages.Categories.Dialogs.AddCategoryOptionsDialog;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Structures.Dialogs;
using WorkoutManager.Contract.Models.Base;
using WorkoutManager.Contract.Models.Categories;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.Pages.Categories.Dialogs.CategoryDialog
{
    internal class CategoryDialogViewModel : DialogModelBase
    {
        public static string DialogsIdentifier => "CategoryDialogDialogs";
        
        private readonly DialogFactory _dialogs;


        #region Commands

        public Command OpenAddOptionsCommand { get; }
        
        public ICommand RemoveOptionCommand { get; }

        #endregion

        
        #region UI Properties

        private CategoryViewModel _category;
        public CategoryViewModel Category
        {
            get => _category;
            set => SetField(ref _category, value);
        }

        public IEnumerable<Type> AllowedCategoryTypes { get; } = new[] { typeof(Exercise), typeof(Muscle) };
        
        #endregion


        #region AddOptionsDialog

        private async void OpenAddOptions()
        {
            var dialog = _dialogs.For<AddCategoryOptionsDialogViewModel>(DialogsIdentifier);
            dialog.Data.SubmitButtonTitle = "Select";
            dialog.Data.DialogTitle = "Add options";
            dialog.Data.Options = _availableOptions;
            
            var dialogResult = await dialog.Show();

            if (dialogResult != DialogResult.Ok)
            {
                return;
            }

            if (!dialog.Data.SelectedOptions.Any())
            {
                return;
            }
           
            Category.AddItems(dialog.Data.SelectedOptions);
        }

        #endregion
        
        
        public CategoryOptions CategoryOptions { private get; set; }

        private IEnumerable<IEntity> _availableOptions = new List<IEntity>();
        
        private IEnumerable<IEntity> GetAvailableOptions() => CategoryOptions.GetOptions(Category.ItemType).Where(IsAvailableOption).ToArray();
        
        private bool IsAvailableOption(IEntity option) => !Category.ContainsItem(option);

        private void UpdateAvailableOptions()
        {
            _availableOptions = GetAvailableOptions();
            OpenAddOptionsCommand.RaiseCanExecuteChanged();
        }
        
        public CategoryDialogViewModel(DialogFactory dialogs)
        {
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Category))
                {
                    Category.Items.CollectionChanged += (o, eventArgs) => UpdateAvailableOptions();
                    
                    Category.PropertyChanged += (s, a) =>
                    {
                        if (a.PropertyName == nameof(Category.ItemType))
                        {
                            UpdateAvailableOptions();
                        }
                    };

                    if (Category.ItemType != null)
                    {
                        UpdateAvailableOptions();
                    }
                }
            };

            OpenAddOptionsCommand = new Command(
                OpenAddOptions,
                () => _availableOptions.Any()
            );
            
            RemoveOptionCommand = new Command<CategoryItemViewModel>(item => Category.Items.Remove(item));

            _dialogs = dialogs;
        }
    }
}