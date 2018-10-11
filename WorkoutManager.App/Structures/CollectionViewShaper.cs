using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Data;

namespace WorkoutManager.App.Structures
{
    internal class CollectionViewShaper<TSource>
    {
        private readonly ICollectionView _view;
        private Predicate<object> _filter;
        private readonly List<SortDescription> _sortDescriptions;
        private readonly List<GroupDescription> _groupDescriptions;
        
        public CollectionViewShaper(ICollectionView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _filter = view.Filter;
            _sortDescriptions = view.SortDescriptions.ToList();
            _groupDescriptions = view.GroupDescriptions.ToList();
        }
     
        public void Apply()
        {
            using (_view.DeferRefresh())
            {
                _view.Filter = _filter;
                _view.SortDescriptions.Clear();
                foreach (var s in _sortDescriptions)
                {
                    _view.SortDescriptions.Add(s);
                }
                _view.GroupDescriptions.Clear();
                foreach (var g in _groupDescriptions)
                {
                    _view.GroupDescriptions.Add(g);
                }
            }
        }
             
        public CollectionViewShaper<TSource> ClearGrouping()
        {
            _groupDescriptions.Clear();
            return this;
        }
     
        public CollectionViewShaper<TSource> ClearSort()
        {
            _sortDescriptions.Clear();
            return this;
        }
     
        public CollectionViewShaper<TSource> ClearFilter()
        {
            _filter = null;
            return this;
        }
     
        public CollectionViewShaper<TSource> ClearAll()
        {
            _filter = null;
            _sortDescriptions.Clear();
            _groupDescriptions.Clear();
            return this;
        }
     
        public CollectionViewShaper<TSource> Where(Func<TSource, bool> predicate)
        {
            _filter = o => predicate((TSource)o);
            return this;
        }
     
        public CollectionViewShaper<TSource> OrderBy<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            return OrderBy(keySelector, true, ListSortDirection.Ascending);
        }
     
        public CollectionViewShaper<TSource> OrderByDescending<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            return OrderBy(keySelector, true, ListSortDirection.Descending);
        }
     
        public CollectionViewShaper<TSource> ThenBy<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            return OrderBy(keySelector, false, ListSortDirection.Ascending);
        }
     
        public CollectionViewShaper<TSource> ThenByDescending<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            return OrderBy(keySelector, false, ListSortDirection.Descending);
        }
     
        private CollectionViewShaper<TSource> OrderBy<TKey>(Expression<Func<TSource, TKey>> keySelector, bool clear, ListSortDirection direction)
        {
            var path = GetPropertyPath(keySelector.Body);
            if (clear)
            {
                _sortDescriptions.Clear();
            }

            _sortDescriptions.Add(new SortDescription(path, direction));
            return this;
        }
     
        public CollectionViewShaper<TSource> GroupBy<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            var path = GetPropertyPath(keySelector.Body);
            _groupDescriptions.Add(new PropertyGroupDescription(path));
            return this;
        }
     
        private static string GetPropertyPath(Expression expression)
        {
            var names = new Stack<string>();
            var expr = expression;
            while (expr != null && !(expr is ParameterExpression) && !(expr is ConstantExpression))
            {
                if (!(expr is MemberExpression memberExpr))
                {
                    throw new ArgumentException("The selector body must contain only property or field access expressions");
                }

                names.Push(memberExpr.Member.Name);
                expr = memberExpr.Expression;
            }
            return string.Join(".", names.ToArray());
        }
    }

    internal static class CollectionViewShaper
    {
        public static CollectionViewShaper<TSource> ShapeView<TSource>(this IEnumerable<TSource> source)
        {
            var view = CollectionViewSource.GetDefaultView(source);
            return new CollectionViewShaper<TSource>(view);
        }
     
        public static CollectionViewShaper<TSource> Shape<TSource>(this ICollectionView view)
        {
            return new CollectionViewShaper<TSource>(view);
        }
    }
}