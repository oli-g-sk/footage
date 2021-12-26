namespace Footage.Messages
{
    public class IsBusyChangedMessage
    {
        public bool IsBusy { get; }

        public IsBusyChangedMessage(bool isBusy)
        {
            IsBusy = isBusy;
        }
    }
}