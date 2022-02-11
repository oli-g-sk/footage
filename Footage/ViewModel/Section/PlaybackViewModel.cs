namespace Footage.ViewModel.Section
{
    using System;
    using System.Threading.Tasks;
    using Footage.Engine;
    using Footage.Messages;
    using Footage.Repository;
    using Footage.Service;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;
    using GalaSoft.MvvmLight.Command;
    using LibVLCSharp.Shared;

    public class PlaybackViewModel : VideoDetailViewModelBase
    {
        private static VideoDetailRepository DetailRepo => Locator.Get<VideoDetailRepository>();

        private readonly IMediaPlayerService player = Locator.Create<IMediaPlayerService>();

        public bool SelectedVideoMissing => SelectedVideo != null && SelectedVideo.IsMissing;

        public bool VideoCanPlay => SelectedVideo != null && !SelectedVideoMissing;

        public long CurrentVideoDuration => player.Duration;

        public float PlaybackProgress
        {
            get => Math.Max(player.Position, 0);
            set
            {
                player.Position = value;
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
            PlayPauseCommand = new RelayCommand(PlayPause, IsMediaLoaded);
            StopCommand = new RelayCommand(Stop, IsMediaLoaded);
            
            player.PositionChanged += Player_PositionChanged;
        }

        protected override void BeforeSelectedVideoChanged()
        {
            // unload the media from the player - otherwise, if a new file would be loaded,
            // the player would display frame from the previous file until the new file is actually played
            player.Stop();
        }

        protected override void AfterSelectedVideoChanged()
        {
            // TODO await
            RaisePropertyChanged(nameof(SelectedVideoMissing));
            RaisePropertyChanged(nameof(VideoCanPlay));
            ReloadSelectedVideo();
        }


        private void PlayPause()
        {
            if (player.IsPlaying)
            {
                player.Pause();
            }
            else
            {
                player.Play();
            }
        }

        private void Stop()
        {
            player.Stop();
            PlaybackProgress = 0;
        }

        private bool IsMediaLoaded()
        {
            return player.IsMediaLoaded;
        }
        
        private async Task ReloadSelectedVideo()
        {
            PlaybackProgress = 0;

            if (SelectedVideoMissing)
            {
                await player.UnloadMedia();
            }
            else if (SelectedVideo != null)
            {
                string? path = DetailRepo.GetVideoPath(SelectedMediaSource.Item, SelectedVideo.Item);
                await DetailRepo.ProcessSelectedVideo(SelectedMediaSource.Item, SelectedVideo.Item);
                await player.LoadMedia(path);
            }

            PlaybackProgress = 0;

            RaisePropertyChanged(nameof(PlaybackPosition));
            RaisePropertyChanged(nameof(PlaybackPositionTimeCode));
            RaisePropertyChanged(nameof(CurrentVideoDuration));
            RaisePropertyChanged(nameof(CurrentVideoDurationTimeCode));

            PlayPauseCommand.RaiseCanExecuteChanged();
            StopCommand.RaiseCanExecuteChanged();
        }

        private void Player_PositionChanged(object? sender, EventArgs e)
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
