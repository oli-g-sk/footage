namespace Footage.Model
{
    using System.Collections.Generic;

    public class Video : Entity
    {
        /// <summary>
        /// Id of the MediaSource it belongs to
        /// </summary>
        public int MediaSourceId { get; set; }
        
        /// <summary>
        /// An unique (in the context of the source) identifier for locating the video.
        /// Locally, this might be its (relative) path, globally, this might be a GUID, a server primary key, etc..
        /// </summary>
        public string MediaSourceUri { get; set; }
        
        public long Duration { get; set; }

        public List<Bookmark> Bookmarks { get; } = new();
    }
}