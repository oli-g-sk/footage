namespace Footage.ViewModel.Section
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Avalonia.Threading;
    using Footage.Model;
    using Footage.Repository;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;
    using GalaSoft.MvvmLight;

    public class VideoBrowserViewModel : SectionViewModel
    {
        private MediaSource? selectedSource;
        
        public ObservableCollection<VideoViewModel> Videos { get; }

        public VideoBrowserViewModel()
        {
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
            using var repo = new VideoBrowserRepository();
            
            var videos = repo.FetchVideos(selectedSource, batchSize);

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