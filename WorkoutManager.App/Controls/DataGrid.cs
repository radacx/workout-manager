using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace WorkoutManager.App.Controls
{
    internal class DataGrid : System.Windows.Controls.DataGrid
    {
        static DataGrid()
        {
            ItemsSourceProperty.OverrideMetadata(typeof(DataGrid), new FrameworkPropertyMetadata(null, OnItemsSourceChanged));    
        }

        private static void OnItemsSourceChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs args)
        {
            if (!(dependencyObject is DataGrid dataGrid))
            {
                return;
            }

            if (args.NewValue is INotifyCollectionChanged collection)
            {
                collection.CollectionChanged += (sender, eventArgs) =>
                {
                    dataGrid.Items.Refresh();
                };
            }
        }

        public DataGrid()
        {
            CanUserSortColumns = true;
            CanUserAddRows = false;
            IsReadOnly = true;
            AutoGenerateColumns = false;
            SelectionMode = DataGridSelectionMode.Single;
            SelectionUnit = DataGridSelectionUnit.FullRow;
        }
    }
}