namespace Footage.ViewModel.Section
{
    using System;
    using Footage.Messages;
    using Footage.Service;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;
    using GalaSoft.MvvmLight.Command;
    using JetBrains.Annotations;
    using LibVLCSharp.Shared;

    public class PlaybackViewModel : SectionViewModel
    {
        private MediaProviderBase? mediaProvider;
        
        public MediaPlayer Player => MediaPlayerService.Instance.MainPlayer;

        private VideoViewModel? selectedVideo;

        [CanBeNull]
        public VideoViewModel? SelectedVideo
        {
            get => selectedVideo;
            set
            {
                BeforeVideoChanged();
                Set(ref selectedVideo, value);
                AfterVideoChanged();
            } 
        }

        private long currentVideoDuration;
        // TODO save video duration to DB
        public long CurrentVideoDuration
        {
            get => currentVideoDuration;
            set
            {
                Set(ref currentVideoDuration, value);
                RaisePropertyChanged(nameof(CurrentVideoDurationTimeCode));
            }
        }

        public float PlaybackProgress
        {
            get => Math.Max(Player.Position, 0);
            set
            {
                Player.Position = value;
                RaisePropertyChanged(nameof(PlaybackProgress));
                RaisePropertyChanged(nameof(PlaybackPositionTimeCode));
            }
        }

        public long PlaybackPosition => (long) (PlaybackProgress * CurrentVideoDuration);

        public string PlaybackPositionTimeCode => LongMillisToTimecode(PlaybackPosition);

        public string CurrentVideoDurationTimeCode => LongMillisToTimecode(CurrentVideoDuration);

        public RelayCommand PlayPauseCommand { get; }
        
        public RelayCommand StopCommand { get; }
        
        public PlaybackViewModel()
        {
            MessengerInstance.Register<SelectionChangedMessage<VideoViewModel>>(this, m => SelectedVideo = m.SelectedItem);
            MessengerInstance.Register<SelectedMesiaSourceChangedMessage>(this, m => mediaProvider = m.MediaProvider);
            
            PlayPauseCommand = new RelayCommand(PlayPause, IsMediaLoaded);
            StopCommand = new RelayCommand(Stop, IsMediaLoaded);
            
            Player.PositionChanged += Player_PositionChanged;
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
            PlaybackProgress = 0;
        }

        private bool IsMediaLoaded()
        {
            return Player.Media != null;
        }
        
        private void BeforeVideoChanged()
        {
            if (Player.Media != null)
            {
                Player.Media.ParsedChanged -= PlayerMedia_ParsedChanged;
            }

            // unload the media from the player - otherwise, if a new file would be loaded,
            // the player would display frame from the previous file until the new file is actually played
            Player.Stop();
        }
        
        private void AfterVideoChanged()
        {
            PlaybackProgress = 0;
            CurrentVideoDuration = 0;

            string? path = SelectedVideo == null ? null : mediaProvider?.GetFullPath(SelectedVideo.Item);
            Player.Media = path != null ? new Media(Locator.LibVlc, new Uri(path)) : null;
            
            PlayPauseCommand.RaiseCanExecuteChanged();
            StopCommand.RaiseCanExecuteChanged();
            
            if (Player.Media != null)
            {
                Player.Media.ParsedChanged += PlayerMedia_ParsedChanged;
            }
        }

        void PlayerMedia_ParsedChanged(object? sender, MediaParsedChangedEventArgs e)
        {
            if (e.ParsedStatus == MediaParsedStatus.Done)
            {
                CurrentVideoDuration = Player.Media.Duration;
            }

            Player.Media.ParsedChanged -= PlayerMedia_ParsedChanged;
        }
        
        private void Player_PositionChanged(object? sender, MediaPlayerPositionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(PlaybackProgress));
            RaisePropertyChanged(nameof(PlaybackPositionTimeCode));
        }

        private static string LongMillisToTimecode(long millis)
        {
            return TimeSpan.FromMilliseconds(millis).ToString(@"hh\:mm\:ss\.fff");
        }
    }
}