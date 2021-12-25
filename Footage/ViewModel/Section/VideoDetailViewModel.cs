namespace Footage.ViewModel.Section
{
    using System;
    using Footage.Messages;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;
    using JetBrains.Annotations;
    using LibVLCSharp.Shared;

    public class VideoDetailViewModel : SectionViewModel
    {
        private MediaPlayer player;
        public MediaPlayer Player
        {
            get => player;
            set => Set(ref player, value);
        }

        private VideoViewModel? selectedVideo;

        [CanBeNull]
        public VideoViewModel? SelectedVideo
        {
            get => selectedVideo;
            set
            {
                Set(ref selectedVideo, value);
                LoadSelectedVideo();
            } 
        }

        public VideoDetailViewModel()
        {
            player = new MediaPlayer(Locator.LibVlc);
            MessengerInstance.Register<SelectionChangedMessage<VideoViewModel>>(this, m => SelectedVideo = m.SelectedItem);
        }

        private void LoadSelectedVideo()
        {
            var uri = new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4");
            
            Player.Media = new Media(Locator.LibVlc, uri);
            Player.Play();
        }
    }
}