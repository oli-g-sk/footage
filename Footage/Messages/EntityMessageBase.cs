namespace Footage.Messages
{
    using Footage.Model;

    public abstract class EntityMessageBase<T> where T : IEntity
    {
        public int Id { get; }
        
        protected EntityMessageBase(IEntity entity)
        {
            Id = entity.Id;
        }
    }
}