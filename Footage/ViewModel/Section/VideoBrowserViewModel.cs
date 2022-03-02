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
    using GalaSoft.MvvmLight.Command;

    public class VideoBrowserViewModel : ItemsViewModel<VideoViewModel, Video>
    {
        private static VideoBrowserRepository Repo => Locator.Get<VideoBrowserRepository>();

        private MediaSource? selectedSource;

        private bool isFetchingVideos;

        public bool IsFetchingVideos
        {
            get => isFetchingVideos;
            set
            {
                Set(ref isFetchingVideos, value);
                FetchMoreCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand FetchMoreCommand { get; }
        
        public VideoBrowserViewModel()
        {
            MessengerInstance.Register<SelectionChangedMessage<MediaSourceViewModel>>(this, OnMediaSourceChanged);
            FetchMoreCommand = new RelayCommand(FetchMore, CanFetchMore);
        }

        private void FetchMore()
        {
            IsFetchingVideos = true;
            
            // TODO await / make async
            Fetch().ContinueWith(t =>
            {
                Dispatcher.InvokeAsync(() =>
                {
                    IsFetchingVideos = false;
                });
            });
        }

        private bool CanFetchMore()
        {
            return !IsFetchingVideos;
        }

        private void OnMediaSourceChanged(SelectionChangedMessage<MediaSourceViewModel> message)
        {
            selectedSource = message.SelectedItem?.Item;
            
            // TODO clear async
            Items.Clear();
            
            // TODO await?
            FetchVideos();
        }

        private async Task FetchVideos()
        {
            if (selectedSource == null)
            {
                return;
            }
            
            IsFetchingVideos = true;
            
            MessengerInstance.Send(new IsBusyChangedMessage(true));
            
            await Repo.UpdateVideoQuery(selectedSource.Id);
            
            await Fetch();

            MessengerInstance.Send(new IsBusyChangedMessage(false));

            IsFetchingVideos = false;
        }

        private async Task Fetch()
        {
            var videos = await Repo.Fetch();

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
        }

        protected override Task DeleteModel(Video item)
        {
            throw new NotImplementedException();
        }
    }
}
