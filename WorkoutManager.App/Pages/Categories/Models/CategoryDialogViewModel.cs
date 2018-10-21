using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils.Dialogs;
using WorkoutManager.Contract.Models.Categories;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Misc;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Pages.Categories.Models
{
    internal class CategoryDialogViewModel : DialogModelBase
    {
        private readonly DialogViewer _dialogViewer;

        public string AddOptionsDialogIdentifier => "AddOptionsDialog";

        public Category Category
        {
            get => _category;
            set => SetField(ref _category, value);
        }

        public ObservedCollection<IEntity> CategoryLinkedItems
        {
            get => _categoryLinkedItems;
            set => SetField(ref _categoryLinkedItems, value);
        }

        public Type SelectedType
        {
            get => _selectedType;
            set => SetField(ref _selectedType, value);
        }

        private IEnumerable<IEntity> _options = new List<IEntity>();

        private IEnumerable<IEntity> Options
        {
            get => _options;
            set
            {
                if (SetField(ref _options, value))
                {
                    OpenAddOptionsCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public Command OpenAddOptionsCommand { get; }
        
        public ICommand RemoveOptionCommand { get; }

        public IEnumerable<Type> AllowedCategoryTypes { get; } = new[] { typeof(Exercise), typeof(Muscle) };
        
        private IEnumerable<Exercise> _exercises;
        private IEnumerable<Muscle> _muscles;
        
        private readonly Repository<Exercise> _exerciseRepository;
        private readonly Repository<Muscle> _muscleRepository;

        private Type _selectedType;
        private ObservedCollection<IEntity> _categoryLinkedItems;
        private Category _category;

        private void LoadData()
        {
            _exercises = _exerciseRepository.GetAll();
            _muscles = _muscleRepository.GetAll();
        }

        private bool IsAvailableOption(IEntity option) => !Category.Items.Contains(option);
        
        private async void OpenAddOptions()
        {
            var dialog = _dialogViewer.For<AddCategoryOptionsDialogViewModel>(AddOptionsDialogIdentifier);
            dialog.Data.SubmitButtonTitle = "Select";
            dialog.Data.DialogTitle = "Add options";
            dialog.Data.Options = _options.Where(IsAvailableOption);
            
            var dialogResult = await dialog.Show();

            if (dialogResult != DialogResult.Ok)
            {
                return;
            }

            if (!dialog.Data.SelectedOptions.Any())
            {
                return;
            }
           
            CategoryLinkedItems.AddRange(dialog.Data.SelectedOptions);
        }

        public CategoryDialogViewModel(Repository<Exercise> exerciseRepository, Repository<Muscle> muscleRepository, DialogViewer dialogViewer)
        {
            PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName) {
                    case nameof(SelectedType): {
                        Category.ItemType = SelectedType;
                        
                        if (SelectedType == typeof(Exercise))
                        {
                            Options = _exercises;
                        }
                        else if (SelectedType == typeof(Muscle))
                        {
                            Options = _muscles;
                        }
                        else
                        {
                            throw new ArgumentException("Invalid type");
                        }

                        break;
                    }
                    case nameof(Category):
                        if (SelectedType != null)
                        {
                            Category.ClearItems();
                        }
                        
                        SelectedType = Category.ItemType;
                    
                        CategoryLinkedItems = new ObservedCollection<IEntity>(
                            Category.Items,
                            Category.AddItem,
                            Category.RemoveItem,
                            OpenAddOptionsCommand.RaiseCanExecuteChanged
                        );
                        
                        break;
                }
            };

            OpenAddOptionsCommand = new Command(
                OpenAddOptions,
                () => Options != null && Options.Where(IsAvailableOption).Any()
            );
            
            RemoveOptionCommand = new Command<IEntity>(option => CategoryLinkedItems.Remove(option));
            
            _exerciseRepository = exerciseRepository;
            _muscleRepository = muscleRepository;
            _dialogViewer = dialogViewer;

            LoadData();
        }
    }
}