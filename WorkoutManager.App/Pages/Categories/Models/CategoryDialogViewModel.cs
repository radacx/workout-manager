using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using WorkoutManager.App.Structures;
using WorkoutManager.Contract.Models.Categories;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Misc;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Pages.Categories.Models
{
    internal class CategoryDialogViewModel : ViewModelBase, INotifyPropertyChanged
    {
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

        public IEnumerable<IEntity> Options
        {
            get => _options;
            set => SetField(ref _options, value);
        }

        public IEnumerable<Type> AllowedCategoryTypes { get; } = new[] { typeof(Exercise), typeof(MuscleGroup) };
        
        private IEnumerable<Exercise> _exercises;
        private IEnumerable<MuscleGroup> _muscleGroups;
        
        private readonly Repository<Exercise> _exerciseRepository;
        private readonly Repository<MuscleGroup> _muscleGroupRepository;
        private IEnumerable<IEntity> _options;
        private Type _selectedType;
        private ObservedCollection<IEntity> _categoryLinkedItems;
        private Category _category;

        private void LoadData()
        {
            _exercises = _exerciseRepository.GetAll();
            _muscleGroups = _muscleGroupRepository.GetAll();

            if (SelectedType.FullName == typeof(Exercise).FullName)
            {
                Options = _exercises;
            }
            else if (SelectedType.FullName == typeof(MuscleGroup).FullName)
            {
                Options = _muscleGroups;
            }
        }
        
        public CategoryDialogViewModel(Repository<Exercise> exerciseRepository, Repository<MuscleGroup> muscleGroupRepository)
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
                        else if (SelectedType == typeof(MuscleGroup))
                        {
                            Options = _muscleGroups;
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
                            Category.RemoveItem
                        );

                        break;
                }
            };
            
            _exerciseRepository = exerciseRepository;
            _muscleGroupRepository = muscleGroupRepository;

            Task.Run(() => LoadData());
        }

        public string SubmitButtonTitle { get; set; }
    }
}