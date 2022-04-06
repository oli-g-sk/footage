namespace Footage.Model
{
    public class Entity : IEntity
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int Id { get; set; }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Entity) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        // TODO remove operator overloads, only use Equals
        public static bool operator ==(Entity? left, Entity? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity? left, Entity? right)
        {
            return !Equals(left, right);
        }

        // ReSharper disable once MemberCanBePrivate.Global
        protected bool Equals(Entity other)
        {
            return Id == other.Id;
        }

        public override string ToString() => $"{GetType().Name} [Id: {Id}]";
    }
}