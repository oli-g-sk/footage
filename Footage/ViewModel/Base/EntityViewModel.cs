namespace Footage.ViewModel.Base
{
    using System;
    using Footage.Messages;
    using Footage.Model;
    using GalaSoft.MvvmLight;

    public class EntityViewModel<T> : ViewModelBase where T : IEntity
    {
        public T Item { get; private set; }
        
        public EntityViewModel(T item)
        {
            Item = item;
            MessengerInstance.Register<EntityUpdatedMessage<T>>(this, OnEntityUpdated);
        }

        private void OnEntityUpdated(EntityUpdatedMessage<T> message)
        {
            if (message.UpdatedEntity.Id == Item.Id)
            {
                Item = message.UpdatedEntity;
                
                // update all ViewModel properties
                RaisePropertyChanged(string.Empty);
            }
        }
    }
}