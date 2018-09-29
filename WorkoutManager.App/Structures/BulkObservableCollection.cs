using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace WorkoutManager.App.Structures
{
    public class BulkObservableCollection<TItem> : AsyncObservableCollection<TItem>
    {
        private const string CountString = "Count";
        private const string IndexerName = "Item[]";
        
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
    }
}