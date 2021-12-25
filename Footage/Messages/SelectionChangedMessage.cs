namespace Footage.Messages
{
    using GalaSoft.MvvmLight.Messaging;

    public class SelectionChangedMessage<T> : MessageBase
    {
        public T? SelectedItem { get; }

        public SelectionChangedMessage(T? selectedItem)
        {
            SelectedItem = selectedItem;
        }
    }
}