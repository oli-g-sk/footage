namespace Footage.ViewModel.Section
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Avalonia.Threading;
    using Footage.Messages;
    using Footage.Model;
    using Footage.Repository;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;

    public class VideoBrowserViewModel : SectionViewModel
    {
        private MediaSource? selectedSource;

        private TaskCompletionSource<bool>? currentFetchTcs;

        public ObservableCollection<VideoViewModel> Videos { get; }

        private VideoViewModel? selectedVideo;

        public VideoViewModel? SelectedVideo
        {
            get => selectedVideo;
            set
            {
                Set(ref selectedVideo, value);
                MessengerInstance.Send(new SelectionChangedMessage<VideoViewModel>(SelectedVideo));
            }
        }

        public VideoBrowserViewModel()
        {
            Videos = new ObservableCollection<VideoViewModel>();
            
            MessengerInstance.Register<SelectionChangedMessage<MediaSourceViewModel>>(this, m => SwitchSource(m.SelectedItem));
        }

        public void SwitchSource(MediaSourceViewModel source)
        {
            selectedSource = source.Item;
            
            // TODO clear async
            Videos.Clear();

            // TODO cancel existing, if any
            Task.Run(async () =>
            {
                if (currentFetchTcs != null)
                {
                    await currentFetchTcs.Task;
                }
                
                return FetchVideos();
            });
        }

        private async Task FetchVideos(int? batchSize = null)
        {
            var initialSource = selectedSource;
            
            currentFetchTcs = new TaskCompletionSource<bool>();
            
            using var repo = new VideoBrowserRepository();
            
            var videos = repo.FetchVideos(selectedSource, batchSize);

            foreach (var video in videos)
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Videos.Add(new VideoViewModel(video));
                });
                
                if (selectedSource != initialSource)
                {
                    Videos.Clear(); // TODO clear async
                    break;
                }
                
#if DEBUG
                await Task.Delay(250);
#endif
            }

            currentFetchTcs.TrySetResult(true);
            currentFetchTcs = null;
        }
    }
}
