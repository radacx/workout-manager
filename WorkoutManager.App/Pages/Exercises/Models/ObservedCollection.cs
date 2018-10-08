using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WorkoutManager.App.Structures;

namespace WorkoutManager.App.Pages.Exercises.Models
{
    internal sealed class ObservedCollection<T>  : BulkObservableCollection<T>
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