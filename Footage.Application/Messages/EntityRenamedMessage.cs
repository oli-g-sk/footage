namespace Footage.Application.Messages
{
    using Footage.Model;

    public class EntityRenamedMessage<T> : EntityMessageBase where T : INamedEntity
    {
        public string? Name { get; }
        
        public EntityRenamedMessage(T updatedEntity) : base(updatedEntity)
        {
            Name = updatedEntity.Name;
        }
    }
}