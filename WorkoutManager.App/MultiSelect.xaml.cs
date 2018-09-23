using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WorkoutManager.App
{
    public partial class MultiSelect : UserControl
    {
        private bool _isInitialized;
        
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(MultiSelect),
            new PropertyMetadata(default(dynamic))
        );

        public IEnumerable ItemsSource
        {
            get => (IEnumerable) GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }  

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
            nameof(SelectedItems),
            typeof(ICollection),
            typeof(MultiSelect),
            new PropertyMetadata(default(ICollection))
        );

        public IList SelectedItems
        {
            get => (IList) GetValue(SelectedItemsProperty);
            set => SetValue(SelectedItemsProperty, value);
        }

        public static readonly DependencyProperty GetItemTextProperty = DependencyProperty.Register(
            nameof(GetItemText),
            typeof(Func<object, string>),
            typeof(MultiSelect),
            new PropertyMetadata(default(IValueConverter))
        );

        public Func<object, string> GetItemText
        {
            get => (Func<object, string>) GetValue(GetItemTextProperty);
            set => SetValue(GetItemTextProperty, value);
        }
        
        public MultiSelect()
        {
            InitializeComponent();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitialized)
            {
                return;
            }
            
            foreach (MultiSelectItem selection in e.AddedItems)
            {
                SelectedItems.Add(selection.Item);
            }

            foreach (MultiSelectItem selection in e.RemovedItems)
            {
                SelectedItems.Remove(selection.Item);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e) => _isInitialized = true;
    }
}
