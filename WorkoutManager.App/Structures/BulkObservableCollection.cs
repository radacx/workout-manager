using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using WorkoutManager.App.Annotations;

namespace WorkoutManager.App.Structures
{
    internal class BulkObservableCollection<TItem> : AsyncObservableCollection<TItem>
    {
        private const string CountString = "Count";
        private const string IndexerName = "Item[]";

        public BulkObservableCollection() { }

        public BulkObservableCollection([NotNull] List<TItem> list) : base(list) { }

        public BulkObservableCollection([NotNull] IEnumerable<TItem> collection) : base(collection) { }

        public void AddRange(IEnumerable<TItem> items)
        {
            var itemsList = items.ToList();
            
            if (!itemsList.Any())
            {
                return;
            }
            
            foreach (var item in itemsList)
            {
                Items.Add(item);
            }
            
            OnPropertyChanged(new PropertyChangedEventArgs(CountString));
            OnPropertyChanged(new PropertyChangedEventArgs(IndexerName));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Replace(Predicate<TItem> predicate, TItem item)
        {
            var itemsList = Items.ToList();
            var itemIndex = itemsList.FindIndex(predicate);
            
            if (itemIndex == -1)
            {
                return;
            }

            base.SetItem(itemIndex, item);  
            
            OnPropertyChanged(new PropertyChangedEventArgs(IndexerName));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        
        public void Replace(TItem originalItem, TItem newItem)
        {
            var itemsList = Items.ToList();
            var itemIndex = itemsList.IndexOf(originalItem);
            
            if (itemIndex == -1)
            {
                return;
            }

            base.SetItem(itemIndex, newItem);  
            
            OnPropertyChanged(new PropertyChangedEventArgs(IndexerName));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}