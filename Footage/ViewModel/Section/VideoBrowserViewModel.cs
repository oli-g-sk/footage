namespace Footage.ViewModel.Section
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Footage.Messages;
    using Footage.Model;
    using Footage.Repository;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;

    public class VideoBrowserViewModel : ItemsViewModel<VideoViewModel, Video>
    {
        private static VideoBrowserRepository Repo => Locator.Get<VideoBrowserRepository>();

        private MediaSource? selectedSource;

        private bool isFetchingVideos;

        public bool IsFetchingVideos
        {
            get => isFetchingVideos;
            set => Set(ref isFetchingVideos, value);
        }

        public VideoBrowserViewModel()
        {
            MessengerInstance.Register<SelectionChangedMessage<MediaSourceViewModel>>(this, OnMediaSourceChanged);
        }

        private void OnMediaSourceChanged(SelectionChangedMessage<MediaSourceViewModel> message)
        {
            selectedSource = message.SelectedItem?.Item;
            
            // TODO clear async
            Items.Clear();
            
            // TODO await?
            FetchVideos();
        }

        private async Task FetchVideos(int? batchSize = null)
        {
            if (selectedSource == null)
            {
                return;
            }
            
            IsFetchingVideos = true;
            
            MessengerInstance.Send(new IsBusyChangedMessage(true));
            
            var videos = await Repo.FetchVideos(selectedSource.Id, batchSize);

            foreach (var video in videos)
            {
                await Dispatcher.InvokeAsync(async () =>
                {
                    Items.Add(new VideoViewModel(video));
                });

#if DEBUG
                await Task.Delay(125);
#endif
            }
            
            MessengerInstance.Send(new IsBusyChangedMessage(false));

            IsFetchingVideos = false;
        }

        protected override Task DeleteModel(Video item)
        {
            throw new NotImplementedException();
        }
    }
}
