namespace Footage.ViewModel.Section
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Avalonia.Threading;
    using Footage.Messages;
    using Footage.Model;
    using Footage.Repository;
    using Footage.Service;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;

    public class VideoBrowserViewModel : SectionViewModel
    {
        private MediaSource? selectedSource;

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
            
            MessengerInstance.Register<SelectedMesiaSourceChangedMessage>(this, m => SwitchSource(m.SelectedItem));
        }

        public async Task SwitchSource(MediaSourceViewModel? source)
        {
            selectedSource = source?.Item;
            
            // TODO clear async
            Videos.Clear();
            
            await FetchVideos();
        }

        private async Task FetchVideos(int? batchSize = null)
        {
            if (selectedSource == null)
            {
                return;
            }
            
            MessengerInstance.Send(new IsBusyChangedMessage(true));
            
            using var repo = new VideoBrowserRepository();
            
            var videos = repo.FetchVideos(selectedSource, batchSize);

            foreach (var video in videos)
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Videos.Add(new VideoViewModel(video));
                });

#if DEBUG
                await Task.Delay(25);
#endif
            }
            
            MessengerInstance.Send(new IsBusyChangedMessage(false));
        }
    }
}
