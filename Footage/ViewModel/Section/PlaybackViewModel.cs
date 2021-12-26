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

        private long currentVideoDuration;

        public long CurrentVideoDuration
        {
            get => currentVideoDuration;
            set => Set(ref currentVideoDuration, value);
        }

        public float PlaybackPosition
        {
            get => Player.Position;
            set
            {
                Player.Position = value;
                RaisePropertyChanged(nameof(PlaybackPosition));
            }
        }

        public RelayCommand PlayPauseCommand { get; }
        
        public RelayCommand StopCommand { get; }
        
        public PlaybackViewModel()
        {
            player = new MediaPlayer(Locator.LibVlc);
            MessengerInstance.Register<SelectionChangedMessage<VideoViewModel>>(this, m => SelectedVideo = m.SelectedItem);
            
            PlayPauseCommand = new RelayCommand(PlayPause, IsMediaLoaded);
            StopCommand = new RelayCommand(Stop, IsMediaLoaded);
            
            Player.MediaChanged += Player_MediaChanged;
            Player.PositionChanged += Player_PositionChanged;
        }

        private void LoadSelectedVideo()
        {
            var uri = new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4");

            Player.Media = SelectedVideo != null ? new Media(Locator.LibVlc, uri) : null;
            PlayPauseCommand.RaiseCanExecuteChanged();
            StopCommand.RaiseCanExecuteChanged();
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

        private void Stop()
        {
            Player.Stop();
            SelectedVideo = null;
        }

        private bool IsMediaLoaded()
        {
            return Player.Media != null;
        }

        private void Player_MediaChanged(object? sender, MediaPlayerMediaChangedEventArgs e)
        {
            PlaybackPosition = 0;
            CurrentVideoDuration = 0;
            
            if (Player.Media != null)
            {
                Player.Media.ParsedChanged += Media_ParsedChanged;
            }

            void Media_ParsedChanged(object? sender, MediaParsedChangedEventArgs e)
            {
                if (e.ParsedStatus == MediaParsedStatus.Done)
                {
                    CurrentVideoDuration = Player.Media.Duration;
                }

                Player.Media.ParsedChanged -= Media_ParsedChanged;
            }
        }

        private void Player_PositionChanged(object? sender, MediaPlayerPositionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(PlaybackPosition));
        }
    }
}