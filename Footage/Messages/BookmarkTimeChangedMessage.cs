namespace Footage.Messages
{
    public class BookmarkTimeChangedMessage
    {
        public long Time { get; }
        
        public BookmarkTimeChangedMessage(long time)
        {
            Time = time;
        }
    }
}