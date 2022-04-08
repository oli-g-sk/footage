namespace Footage.Application.Messages
{
    using Footage.Model;

    // TODO REFACTOR use ViewModel-level entity type, not model?
    public class EntityDeletedMessage<T> where T : Entity
    {
        public T DeletedEntity;
        
        public EntityDeletedMessage(T deletedEntity)
        {
            DeletedEntity = deletedEntity;
        }
    }
}