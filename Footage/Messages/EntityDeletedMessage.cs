namespace Footage.Messages
{
    using Footage.Model;
    using Footage.ViewModel.Base;

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