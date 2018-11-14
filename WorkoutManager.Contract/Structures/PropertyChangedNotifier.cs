using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WorkoutManager.Contract.Annotations;

namespace WorkoutManager.Contract.Structures
{
    public abstract class PropertyChangedNotifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
            => SetField(ref field, value, EqualityComparer<T>.Default, propertyName);

        protected bool SetField<T>(ref T field, T value, IEqualityComparer<T> comparer, [CallerMemberName] string propertyName = null)
        {
            if (comparer.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);

            return true;
        }
    }
}