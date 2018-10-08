using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using WorkoutManager.App.Annotations;

namespace WorkoutManager.App.Structures
{
    internal class AsyncObservableCollection<TItem> : ObservableCollection<TItem>
    {
        public AsyncObservableCollection() { }

        public AsyncObservableCollection([NotNull] List<TItem> list) : base(list) { }

        public AsyncObservableCollection([NotNull] IEnumerable<TItem> collection) : base(collection) { }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
            => Application.Current.Dispatcher.BeginInvoke(
                new Action(() => base.OnCollectionChanged(e)));

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
            => Application.Current.Dispatcher.BeginInvoke(
                new Action(() => base.OnPropertyChanged(e)));
    }
}