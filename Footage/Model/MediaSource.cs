namespace Footage.Model
{
    public class MediaSource : Entity
    {
        public string Name { get; set; }
        
        public bool Active { get; set; }
        
        public bool Available { get; set; }
    }
}