namespace Footage.ViewModel.Section
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Footage.Messages;
    using Footage.Model;
    using Footage.ModelHelper;
    using Footage.Repository;
    using Footage.Service;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;
    using Footage.ViewModel.Helper;
    using GalaSoft.MvvmLight.Command;

    public class VideoBrowserViewModel : ItemsViewModel<VideoViewModel, Video>
    {
        private static IThumbnailManager ThumbnailManager => Locator.Get<IThumbnailManager>();
        
        private static VideoBrowserRepository Repo => Locator.Get<VideoBrowserRepository>();

        private MediaSource? selectedSource;

        private BookmarkFilter bookmarkFilter;

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

        public BookmarkFilterViewModel BookmarkFilter { get; private set; }

        public RelayCommand FetchMoreCommand { get; }
        
        public VideoBrowserViewModel()
        {
            MessengerInstance.Register<SelectionChangedMessage<MediaSourceViewModel>>(this, OnMediaSourceChanged);
            FetchMoreCommand = new RelayCommand(FetchMore, CanFetchMore);

            bookmarkFilter = new BookmarkFilter();
            BookmarkFilter = new BookmarkFilterViewModel(bookmarkFilter);
            BookmarkFilter.FilterChanged += (_, _) => OnFilterChanged();
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
            
            // TODO await?
            ReloadVideos();
        }

        private async Task ReloadVideos()
        {
            // TODO clear async
            Items.Clear();
            
            if (selectedSource == null)
            {
                return;
            }
            
            IsFetchingVideos = true;
            
            MessengerInstance.Send(new IsBusyChangedMessage(true));
            
            await Repo.UpdateVideoQuery(selectedSource.Id, bookmarkFilter);
            
            await Fetch();

            MessengerInstance.Send(new IsBusyChangedMessage(false));

            IsFetchingVideos = false;
        }

        private async Task Fetch()
        {
            var videos = await Repo.Fetch();

            foreach (var video in videos)
            {
                Dispatcher.InvokeAsync(async () =>
                {
                    var viewModel = new VideoViewModel(video);
                    Items.Add(viewModel);

                    string? thumbnail = await ThumbnailManager.GetThumbnail(video);
                    viewModel.ThumbnailPath = thumbnail;
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

        private void OnFilterChanged()
        {
            ReloadVideos();
        }
    }
}
