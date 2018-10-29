using System;
using System.Collections.Generic;
using System.Linq;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Misc;

namespace WorkoutManager.Contract.Models.Categories
{
    public class Category : IEntity, IEquatable<Category>
    {
        private List<IEntity> _items = new List<IEntity>();
        public int Id { get; set; }

        public string Name { get; set; }
        
        public Type ItemType { get; set; }

        public IEntity[] Items
        {
            get => _items.ToArray();
            set => _items = value.ToList();
        }

        public void AddItem(IEntity item) => _items.Add(item);

        public void RemoveItem(IEntity item) => _items.RemoveByReference(item);

        public void ClearItems() => _items.Clear();
        
        public bool Equals(Category other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id == other.Id;
        }

        public bool Equals(IEntity other) => other is Category category && Equals(category);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((Category) obj);
        }

        public override int GetHashCode() => Id;

        public override string ToString() => Name;
    }
}