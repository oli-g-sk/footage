namespace Footage.ViewModel.Section
{
    using System;
    using System.Threading.Tasks;
    using Footage.Messages;
    using Footage.Repository;
    using Footage.Service;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;
    using GalaSoft.MvvmLight.Command;
    using LibVLCSharp.Shared;

    public class PlaybackViewModel : SectionViewModel
    {
        private static VideoBrowserRepository BrowserRepo => Locator.Get<VideoBrowserRepository>();
        private static VideoDetailRepository DetailRepo => Locator.Get<VideoDetailRepository>();

        private readonly IMediaPlayerService mediaPlayerService = Locator.Create<IMediaPlayerService>();

        private MediaSourceViewModel? selectedMediaSource;

        private VideoViewModel? selectedVideo;
        
        // TODO later abstract away entire LibVLC dependency to IMediaPlayerService (using our own IMediaPlayer)
        public MediaPlayer Player => mediaPlayerService.Player;

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
        // TODO save video duration to DB?
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
            MessengerInstance.Register<SelectionChangedMessage<MediaSourceViewModel>>(this, OnMediaSourceChanged);
            
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
            // unload the media from the player - otherwise, if a new file would be loaded,
            // the player would display frame from the previous file until the new file is actually played
            Player.Stop();
        }
        
        private async Task AfterVideoChanged()
        {
            PlaybackProgress = 0;
            CurrentVideoDuration = 0;

            if (SelectedVideo == null)
            {
                await mediaPlayerService.UnloadMedia();
            }
            else 
            {
                string? path = DetailRepo.GetVideoPath(selectedMediaSource.Item, SelectedVideo.Item);
                await mediaPlayerService.LoadMedia(path);
            }

            PlaybackProgress = 0;

            RaisePropertyChanged(nameof(PlaybackPosition));
            RaisePropertyChanged(nameof(PlaybackPositionTimeCode));
            RaisePropertyChanged(nameof(CurrentVideoDuration));
            RaisePropertyChanged(nameof(CurrentVideoDurationTimeCode));

            PlayPauseCommand.RaiseCanExecuteChanged();
            StopCommand.RaiseCanExecuteChanged();
        }

        private void OnMediaSourceChanged(SelectionChangedMessage<MediaSourceViewModel> message)
        {
            selectedMediaSource = message.SelectedItem;
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
