namespace Footage.ViewModel.Section
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading.Tasks;
    using Footage.Model;
    using Footage.Repository;
    using Footage.ViewModel.Entity;
    using GalaSoft.MvvmLight.Command;

    public class BookmarksViewModel : VideoDetailViewModelBase
    {
        private static BookmarksRepository Repo => Locator.Get<BookmarksRepository>();
        
        public ObservableCollection<BookmarkViewModel> Bookmarks { get; }

        public ObservableCollection<BookmarkViewModel> SelectedBookmarks { get; } = new();
        
        public RelayCommand<PlaybackViewModel> AddTimeBookmarkCommand { get; }
        
        public RelayCommand RemoveSelectedBookmarksCommand { get; }
        
        public BookmarksViewModel()
        {
            Bookmarks = new ObservableCollection<BookmarkViewModel>();
            AddTimeBookmarkCommand = new RelayCommand<PlaybackViewModel>(AddTimeBookmark, _ => SelectedVideo != null);
            RemoveSelectedBookmarksCommand = new RelayCommand(RemoveSelectedBookmarks, () => SelectedBookmarks.Any());
            
            SelectedBookmarks.CollectionChanged += SelectedBookmarks_CollectionChanged;
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
        
        private void SelectedBookmarks_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RemoveSelectedBookmarksCommand.RaiseCanExecuteChanged();
        }
    }
}
