namespace Footage.ViewModel.Section
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading.Tasks;
    using Footage.Messages;
    using Footage.Model;
    using Footage.Presentation;
    using Footage.Repository;
    using Footage.ViewModel.Entity;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    public class BookmarksViewModel : VideoDetailViewModelBase
    {
        private static BookmarksRepository Repo => Locator.Get<BookmarksRepository>();
        
        public ObservableCollectionEx<BookmarkViewModel> Bookmarks { get; }

        public ObservableCollection<BookmarkViewModel> SelectedBookmarks { get; } = new();
        
        public RelayCommand<PlaybackViewModel> AddTimeBookmarkCommand { get; }
        
        public RelayCommand RemoveSelectedBookmarksCommand { get; }
        
        public BookmarksViewModel()
        {
            Bookmarks = new ObservableCollectionEx<BookmarkViewModel>();
            AddTimeBookmarkCommand = new RelayCommand<PlaybackViewModel>(AddTimeBookmark, _ => SelectedVideo != null);
            RemoveSelectedBookmarksCommand = new RelayCommand(RemoveSelectedBookmarks, () => SelectedBookmarks.Any());
            
            SelectedBookmarks.CollectionChanged += SelectedBookmarks_CollectionChanged;
            
            Bookmarks.ForNewItems(i => i.TimeChanged += Bookmark_TimeChanged);
            Bookmarks.ForOldItems(i => i.TimeChanged -= Bookmark_TimeChanged);
        }

        // TODO make async
        public void AddTimeBookmark(PlaybackViewModel playbackViewModel)
        {
            // TODO await
            var task = Repo.AddTimeBookmarkToVideo(SelectedVideo.Item, playbackViewModel.PlaybackPosition);
            task.Wait();
            var bookmark = task.Result;
            Bookmarks.Add(new TimeBookmarkViewModel(bookmark));
        }

        private void RemoveSelectedBookmarks()
        {
            var selection = new List<BookmarkViewModel>(SelectedBookmarks);
            foreach (var bookmark in selection)
            {
                Bookmarks.Remove(bookmark);
            }

            // TODO await
            Repo.RemoveBookmarks(SelectedVideo.Item, selection.Select(b => b.Item));
            
            // TODO await
            SelectedBookmarks.Clear();
        }

        private void SaveBookmarks()
        {
            // TODO await
            var task = Repo.UpdateBookmarkTimes(Bookmarks.Select(b => b.Item));
            task.Wait();
        }

        private async Task LoadBookmarks()
        {
            if (SelectedVideo == null)
            {
                return;
            }
            
            foreach (var bookmark in SelectedVideo.Item.Bookmarks)
            {
                BookmarkViewModel viewModel = bookmark is RangeBookmark rb
                    ? new RangeBookmarkViewModel(rb)
                    : new TimeBookmarkViewModel((bookmark as TimeBookmark)!);
                
                await Dispatcher.InvokeAsync(() =>
                {
                    Bookmarks.Add(viewModel);
                });
            }
        }

        protected override void BeforeSelectedVideoChanged()
        {
            SaveBookmarks();

            // TODO clear async
            Bookmarks.Clear();
        }

        protected override void AfterSelectedVideoChanged()
        {
            AddTimeBookmarkCommand.RaiseCanExecuteChanged();
            RemoveSelectedBookmarksCommand.RaiseCanExecuteChanged();

            // TODO await
            LoadBookmarks();
        }

        private void Bookmark_TimeChanged(long time)
        {
            MessengerInstance.Send(new BookmarkTimeChangedMessage(time));
        }

        private void SelectedBookmarks_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RemoveSelectedBookmarksCommand.RaiseCanExecuteChanged();
        }
    }
}
