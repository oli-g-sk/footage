namespace Footage.ViewModel.Section
{
    using System;
    using Footage.Messages;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;
    using GalaSoft.MvvmLight.Command;
    using JetBrains.Annotations;
    using LibVLCSharp.Shared;

    public class PlaybackViewModel : SectionViewModel
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
        
        public RelayCommand PlayPauseCommand { get; }
        
        public PlaybackViewModel()
        {
            player = new MediaPlayer(Locator.LibVlc);
            MessengerInstance.Register<SelectionChangedMessage<VideoViewModel>>(this, m => SelectedVideo = m.SelectedItem);
            
            PlayPauseCommand = new RelayCommand(PlayPause, CanPlayPause);
        }

        private void LoadSelectedVideo()
        {
            var uri = new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4");

            Player.Media = SelectedVideo != null ? new Media(Locator.LibVlc, uri) : null;
            PlayPauseCommand.RaiseCanExecuteChanged();
        }

        private void PlayPause()
        {
            if (Player.IsPlaying)
            {
                Player.Pause();
            }
            else
            {
                Player.Play();
            }
        }

        private bool CanPlayPause()
        {
            return Player.Media != null;
        }
    }
}