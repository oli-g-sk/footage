namespace Footage.ViewModel.Base
{
    using System;
    using Footage.Messages;
    using Footage.Model;
    using GalaSoft.MvvmLight;

    public class EntityViewModel<T> : ViewModelBase where T : IEntity
    {
        public int Id => Item.Id;
        
        public T Item { get; }
        
        public EntityViewModel(T item)
        {
            Item = item;
        }
    }
}