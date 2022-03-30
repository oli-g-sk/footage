namespace Footage.Messages
{
    using Footage.Model;

    public abstract class EntityMessageBase
    {
        public int EntityId { get; }
        
        protected EntityMessageBase(IEntity entity)
        {
            EntityId = entity.Id;
        }
    }
}