namespace Footage.Messages
{
    using Footage.Model;

    public abstract class EntityMessageBase
    {
        public int Id { get; }
        
        protected EntityMessageBase(IEntity entity)
        {
            Id = entity.Id;
        }
    }
}