namespace Footage.Messages
{
    using GalaSoft.MvvmLight.Messaging;

    public class SelectionChangedMessage<T> : MessageBase
    {
        public T? PreviousItem { get; set; }
        
        public T? SelectedItem { get; }

        public SelectionChangedMessage(T? previousItem, T? selectedItem)
        {
            SelectedItem = selectedItem;
        }
    }
}