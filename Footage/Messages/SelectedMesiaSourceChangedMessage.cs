namespace Footage.Messages
{
    using Footage.Service;
    using Footage.ViewModel.Entity;

    public class SelectedMesiaSourceChangedMessage : SelectionChangedMessage<MediaSourceViewModel>
    {
        public MediaProviderBase? MediaProvider { get; }
        
        public SelectedMesiaSourceChangedMessage(MediaSourceViewModel? selectedItem, MediaProviderBase? mediaProvider) : base(selectedItem)
        {
            MediaProvider = mediaProvider;
        }
    }
}