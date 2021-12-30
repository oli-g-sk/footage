namespace Footage.Model
{
    public abstract class Bookmark : Entity
    {
        public Video Video { get; set; }
    }
}