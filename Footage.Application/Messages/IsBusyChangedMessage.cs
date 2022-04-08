namespace Footage.Application.Messages
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