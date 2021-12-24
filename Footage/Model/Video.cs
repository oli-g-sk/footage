namespace Footage.Model
{
    using System.Collections.Generic;

    public class Video
    {
        public int Id { get; set; }
        
        public string Uri { get; set; }
        
        public long Duration { get; set; }

        public List<Bookmark> Bookmarks { get; } = new();
    }
}