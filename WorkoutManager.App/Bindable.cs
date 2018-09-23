using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace WorkoutManager.App
{
    public class Bindable
    {
        public static readonly DependencyProperty InitializeSelectedItemsProperty = DependencyProperty.RegisterAttached(
            "InitializeSelectedItems", typeof(bool), typeof(Bindable), new PropertyMetadata(default(bool), InitializeSelectedItemsChanged));

        private static void InitializeSelectedItemsChanged(DependencyObject depObject, DependencyPropertyChangedEventArgs args)
        {
            if (depObject is ListBox listView && (bool)args.NewValue)
            {
                TypeDescriptor.GetProperties(listView)["ItemsSource"]
                    .AddValueChanged(listView, ListViewItemsSourceChanged);
            }
        }

        private static void ListViewItemsSourceChanged(object sender, EventArgs eventArgs)
        {
            if (!(sender is ListBox listView))
            {
                return;
            }

            listView.SelectedItems.Clear();

            foreach (var item in listView.ItemsSource/*.OfType<ISelectable>().Where(i => i.IsSelected)*/)
            {
                listView.SelectedItems.Add(item);
            }
        }
    }
}