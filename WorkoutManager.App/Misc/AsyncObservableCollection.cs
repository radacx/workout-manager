using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace WorkoutManager.App.Misc
{
    public class AsyncObservableCollection<TItem> : ObservableCollection<TItem>
    {
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
            => Application.Current.Dispatcher.BeginInvoke(
                new Action(() => base.OnCollectionChanged(e)));

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
            => Application.Current.Dispatcher.BeginInvoke(
                new Action(() => base.OnPropertyChanged(e)));
    }
}