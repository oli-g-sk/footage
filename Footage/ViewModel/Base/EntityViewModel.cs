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

        protected void RegisterToEntityMessage<TMessage>(Action<TMessage> action) where TMessage : EntityMessageBase
        {
            MessengerInstance.Register<TMessage>(this, message =>
            {
                if (message.Id == Id)
                {
                    action.Invoke(message);
                }
            });
        }
    }
}