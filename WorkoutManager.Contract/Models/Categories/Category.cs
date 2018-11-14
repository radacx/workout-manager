using System;
using System.Collections.Generic;
using System.Linq;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Base;

namespace WorkoutManager.Contract.Models.Categories
{
    public class Category : Entity
    {
        private List<EntityReference> _items = new List<EntityReference>();

        public string Name { get; set; }
        
        public Type ItemType { get; set; }

        public EntityReference[] Items
        {
            get => _items.ToArray();
            set => _items = value.ToList();
        }

        public void AddItem(EntityReference item) => _items.Add(item);

        public void RemoveItem(int itemId) => _items.RemoveAll(item => item.Id == itemId);

        public override string ToString() => Name;

        public override IEntity GenericClone() => new Category
        {
            _items = _items.Select(x => x.Clone()).ToList(),
            Name = Name,
            ItemType = ItemType,
            Id = Id,
        };
        
        public Category Clone() => GenericClone() as Category;
    }
}