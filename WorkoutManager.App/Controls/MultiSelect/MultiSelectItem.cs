using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WorkoutManager.App.Annotations;

namespace WorkoutManager.App.Controls.MultiSelect
{
    internal class MultiSelectItem : INotifyPropertyChanged, IEquatable<MultiSelectItem>
    {
        private bool _isSelected;
        private object _item;
        
        public bool IsSelected
        {
            get => _isSelected;
            set => SetField(ref _isSelected, value);
        }

        public object Item
        {
            get => _item;
            set => SetField(ref _item, value);
        }

        public MultiSelectItem(object item, bool isSelected)
        {
            _item = item;
            _isSelected = isSelected;
        }

        private void SetField<T>(
            ref T field,
            T value,
            [CallerMemberName] string propertyName = null
        )
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return;
            }

            field = value;
            OnPropertyChanged(propertyName);
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Equals(MultiSelectItem other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            return ReferenceEquals(this, other) || Equals(_item, other._item);
        }

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

            return obj.GetType() == GetType() && Equals((MultiSelectItem) obj);
        }

        public override int GetHashCode() => _item != null ? _item.GetHashCode() : 0;
    }
}