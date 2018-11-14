using System;
using System.Collections.Generic;
using System.Linq;
using WorkoutManager.App.Structures.Collections.Common;
using WorkoutManager.App.Structures.ViewModels;
using WorkoutManager.App.Validation.Validators;
using WorkoutManager.Contract.Models.Base;
using WorkoutManager.Contract.Models.Categories;

namespace WorkoutManager.App.Pages.Categories.Dialogs.CategoryDialog
{
    internal class CategoryItemViewModel
    {
        public IEntity Entity { get; }

        public string Name => $"{Entity}";

        public CategoryItemViewModel(IEntity entity)
        {
            Entity = entity;
        }
    }
    
    internal class CategoryViewModel : ViewModelBase
    {
        private readonly int _id;
        
        private string _name;
        private Type _itemType;

        public string Name
        {
            get => _name;
            set
            {
                var validator = new TextLengthValidator(minimum: 1);
                var validationResults = validator.Validate(value);
                SetValidationResults(validationResults);

                if (!SetField(ref _name, value) || validationResults != null) { }
            }
        }

        public Type ItemType
        {
            get => _itemType;
            set
            {
                var validationResults = value == null ? new[] { "Select category type." } : null;
                SetValidationResults(validationResults);
                
                if (SetField(ref _itemType, value) || validationResults != null) {}
            }
        }

        public ObservedCollection<CategoryItemViewModel> Items { get; set; }

        public void AddItems(IEnumerable<IEntity> items)
        {
            Items.AddRange(items.Select(item => new CategoryItemViewModel(item)).ToArray());
        }

        public bool ContainsItem(IEntity item) => Items.Any(x => x.Entity.Equals(item));
        
        private CategoryViewModel(int id)
        {
            _id = id;
        }

        public static CategoryViewModel FromModel(Category model, Func<int, Type, IEntity> getOption)
        {
            var viewModel = new CategoryViewModel(model.Id)
            {
                Name = model.Name,
                ItemType = model.ItemType,
                Items = new ObservedCollection<CategoryItemViewModel>(
                    model.Items.Select(reference => new CategoryItemViewModel(getOption(reference.Id, model.ItemType))),
                    itemViewModel => model.AddItem(new EntityReference(itemViewModel.Entity.Id)),
                    itemViewModel => model.RemoveItem(itemViewModel.Entity.Id)
                ),
            };

            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(ItemType))
                {
                    viewModel.Items.Clear();
                }
            };
            
            return viewModel;
        }

        public Category ToModel() => new Category
        {
            Id = _id,
            Name = Name,
            ItemType = ItemType,
            Items = Items.Select(x => new EntityReference(x.Entity.Id)).ToArray(),
        };
    }
}