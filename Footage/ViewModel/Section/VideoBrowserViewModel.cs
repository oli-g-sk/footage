namespace Footage.ViewModel.Section
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Avalonia.Threading;
    using Footage.Model;
    using Footage.Repository;
    using Footage.ViewModel.Entity;
    using GalaSoft.MvvmLight;

    public class VideoBrowserViewModel : ViewModelBase
    {
        private MediaSource? selectedSource;
        
        private readonly VideoBrowserRepository videoRepository;
        
        public ObservableCollection<VideoViewModel> Videos { get; }

        public VideoBrowserViewModel(VideoBrowserRepository videoRepository)
        {
            this.videoRepository = videoRepository;
            Videos = new ObservableCollection<VideoViewModel>();
        }

        public async Task SwitchSource(MediaSourceViewModel source)
        {
            selectedSource = source.Item;
            
            // TODO clear async
            Videos.Clear();
            
            await FetchVideos();
        }

        private async Task FetchVideos(int? batchSize = null)
        {
            var videos = videoRepository.FetchVideos(selectedSource, batchSize);

            foreach (var video in videos)
            {
                await Dispatcher.UIThread.InvokeAsync(async () =>
                {
#if DEBUG
                    await Task.Delay(250);
#endif
                    Videos.Add(new VideoViewModel(video));
                });
            }
        }
    }
}