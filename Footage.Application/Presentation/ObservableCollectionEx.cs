namespace Footage.Application.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    public class ObservableCollectionEx<T> : ObservableCollection<T>
    {
        private Action<T> actionForNewItems;
        private Action<T> actionForOldItems;
        
        public event EventHandler<ItemsChangedEventArgs<T>> ItemsChanged;

        public ObservableCollectionEx()
        {
            CollectionChanged += OnCollectionChanged;   
        }

        public void ForNewItems(Action<T> action)
        {
            actionForNewItems = action;
        }

        public void ForOldItems(Action<T> action)
        {
            actionForOldItems = action;
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            var newItems = new List<T>();
            var oldItems = new List<T>();

            if (e.NewItems != null)
            {
                foreach (var newItem in e.NewItems)
                {
                    if (newItem is T t)
                    {
                        newItems.Add(t);
                        actionForNewItems.Invoke(t);
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (var oldItem in e.OldItems)
                {
                    if (oldItem is T t)
                    {
                        oldItems.Add(t);
                        actionForOldItems.Invoke(t);
                    }
                }
            }

            ItemsChanged?.Invoke(this, new ItemsChangedEventArgs<T>(newItems, oldItems));
        }
    }
    
    public class ItemsChangedEventArgs<T> : EventArgs
    {
        public IEnumerable<T> NewItems;

        public IEnumerable<T> OldItems;

        public ItemsChangedEventArgs(IEnumerable<T> newItems, IEnumerable<T> oldItems)
        {
            NewItems = new List<T>(newItems);
            OldItems = new List<T>(oldItems);
        }
    }
}