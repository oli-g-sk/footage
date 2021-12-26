namespace Footage.Model
{
    public class VideoThumbnail : Entity
    {
        public int VideoId { get; set; }
        
        public byte[] Image { get; set; }
    }
}