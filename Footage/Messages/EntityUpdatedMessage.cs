namespace Footage.Messages
{
    using Footage.Model;

    public class EntityUpdatedMessage<T> where T : IEntity
    {
        public T UpdatedEntity { get; }
        
        public EntityUpdatedMessage(T updatedEntity)
        {
            UpdatedEntity = updatedEntity;
        }
    }
}