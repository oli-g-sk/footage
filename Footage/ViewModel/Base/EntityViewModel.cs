namespace Footage.ViewModel.Base
{
    using System;
    using Footage.Model;
    using GalaSoft.MvvmLight;

    public class EntityViewModel<T> : ViewModelBase where T : Entity
    {
        public T Item { get; }
        
        public EntityViewModel(T item)
        {
            Item = item;
            Item.EntryUpdated += Item_EntryUpdated;
        }

        private void Item_EntryUpdated(object? sender, EventArgs e)
        {
            RaisePropertyChanged(string.Empty);
        }
    }
}