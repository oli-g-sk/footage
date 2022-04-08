namespace Footage.Application.ViewModel.Section
{
    using System;
    using System.Threading.Tasks;
    using Footage.Application.Messages;
    using Footage.Application.Repository;
    using Footage.Engine;
    using GalaSoft.MvvmLight.Command;

    public class PlaybackViewModel : VideoDetailViewModelBase
    {
        private static VideoDetailRepository DetailRepo => Locator.Get<VideoDetailRepository>();

        public IMediaPlayer Player { get; } = Locator.Create<IMediaPlayer>();

        public bool SelectedVideoMissing => SelectedVideo != null && SelectedVideo.IsMissing;

        public bool VideoCanPlay => SelectedVideo != null && !SelectedVideoMissing;

        public long CurrentVideoDuration => Player.Duration;

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

        public TimeSpan PlaybackPositionTimeCode => TimeSpan.FromMilliseconds(PlaybackPosition);

        public TimeSpan CurrentVideoDurationTimeCode => TimeSpan.FromMilliseconds(CurrentVideoDuration);

        public int Volume
        {
            get => Player.Volume;
            set
            {
                Player.Volume = value;
                RaisePropertyChanged(nameof(Volume));
            }
        }

        public RelayCommand PlayPauseCommand { get; }
        
        public RelayCommand StopCommand { get; }
        
        public PlaybackViewModel()
        {            
            PlayPauseCommand = new RelayCommand(PlayPause, IsMediaLoaded);
            StopCommand = new RelayCommand(Stop, IsMediaLoaded);
            
            Player.PositionChanged += Player_PositionChanged;
            MessengerInstance.Register<BookmarkTimeChangedMessage>(this, OnBookmarkTimeChanged);

            Volume = 50;
        }

        private void OnBookmarkTimeChanged(BookmarkTimeChangedMessage message)
        {
            if (Player.IsPlaying)
            {
                Player.Pause();
            }

            float position = message.Time / (float) CurrentVideoDuration;
            Player.Position = position;
            RaisePropertyChanged(nameof(PlaybackProgress));
        }

        protected override void BeforeSelectedVideoChanged()
        {
            // unload the media from the player - otherwise, if a new file would be loaded,
            // the player would display frame from the previous file until the new file is actually played
            Player.Stop();
        }

        protected override void AfterSelectedVideoChanged()
        {
            ReloadSelectedVideo();
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
            return Player.IsMediaLoaded;
        }
        
        private async Task ReloadSelectedVideo()
        {
            PlaybackProgress = 0;

            if (SelectedVideo == null || SelectedVideoMissing)
            {
                await Player.UnloadMedia();
            }
            else if (SelectedVideo != null)
            {
                int videoId = SelectedVideo.Item.Id; 
                string? path = await DetailRepo.GetVideoPath(videoId);
                await DetailRepo.ProcessVideo(videoId);
                await Player.LoadMedia(path);
            }

            RaisePropertyChanged(nameof(PlaybackPosition));
            RaisePropertyChanged(nameof(PlaybackPositionTimeCode));
            RaisePropertyChanged(nameof(CurrentVideoDuration));
            RaisePropertyChanged(nameof(CurrentVideoDurationTimeCode));
            RaisePropertyChanged(nameof(SelectedVideoMissing));
            RaisePropertyChanged(nameof(VideoCanPlay));

            PlayPauseCommand.RaiseCanExecuteChanged();
            StopCommand.RaiseCanExecuteChanged();
        }

        private void Player_PositionChanged(object? sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(PlaybackProgress));
            RaisePropertyChanged(nameof(PlaybackPositionTimeCode));
        }
    }
}
