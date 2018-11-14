using System;
using System.Collections.Generic;

namespace WorkoutManager.App.Structures.Collections.Common
{
    internal class ObservedCollection<T>  : WpfObservableRangeCollection<T>
    {
        public ObservedCollection(IEnumerable<T> values, Action<T> onAdd, Action<T> onRemove) : base(values)
        {
            CollectionChanged += (sender, args) =>
            {
                if (args.NewItems != null)
                {
                    foreach (T value in args.NewItems)
                    {
                        onAdd(value);
                    }
                }

                if (args.OldItems != null)
                {
                    foreach (T value in args.OldItems)
                    {
                        onRemove(value);
                    }
                }
            };
        }
    }
}