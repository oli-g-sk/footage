namespace Footage.Application.Messages
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