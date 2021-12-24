namespace Footage.Model
{
    using System.Collections.Generic;

    public class Video : Entity
    {
        public int MediaSourceId { get; set; }
        
        public string Uri { get; set; }
        
        public long Duration { get; set; }

        public List<Bookmark> Bookmarks { get; } = new();
    }
}