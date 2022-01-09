namespace Footage.ViewModel.Section
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Linq;
    using Avalonia.Threading;
    using Footage.Messages;
    using Footage.Model;
    using Footage.Repository;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;
    using GalaSoft.MvvmLight.Command;

    public class BookmarksViewModel : SectionViewModel
    {
        private VideoViewModel? selectedVideo;
        
        public ObservableCollection<BookmarkViewModel> Bookmarks { get; }

        public ObservableCollection<BookmarkViewModel> SelectedBookmarks { get; } = new();
        
        public RelayCommand<PlaybackViewModel> AddTimeBookmarkCommand { get; }
        
        public RelayCommand RemoveSelectedBookmarksCommand { get; }
        
        public BookmarksViewModel()
        {
            Bookmarks = new ObservableCollection<BookmarkViewModel>();
            AddTimeBookmarkCommand = new RelayCommand<PlaybackViewModel>(AddTimeBookmark, _ => selectedVideo != null);
            RemoveSelectedBookmarksCommand = new RelayCommand(RemoveSelectedBookmarks, () => SelectedBookmarks.Any());
            MessengerInstance.Register<SelectionChangedMessage<VideoViewModel>>(this, OnSelectedVideoChanged);
            
            SelectedBookmarks.CollectionChanged += SelectedBookmarks_CollectionChanged;
        }

        // TODO make async
        public void AddTimeBookmark(PlaybackViewModel playbackViewModel)
        {
            using var repo = new BookmarksRepository();
            // TODO await
            var task = repo.AddTimeBookmarkToVideo(selectedVideo.Item, playbackViewModel.PlaybackPosition);
            task.Wait();
            var bookmark = task.Result;
            Bookmarks.Add(new TimeBookmarkViewModel(bookmark));
        }

        private void RemoveSelectedBookmarks()
        {
            using var repo = new BookmarksRepository();

            var selection = new List<BookmarkViewModel>(SelectedBookmarks);
            foreach (var bookmark in selection)
            {
                Bookmarks.Remove(bookmark);
            }
            
            // TODO await
            repo.RemoveBookmarks(selectedVideo.Item, selection.Select(b => b.Item));
            
            // TODO await
            SelectedBookmarks.Clear();
        }
        
        private void SelectedBookmarks_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RemoveSelectedBookmarksCommand.RaiseCanExecuteChanged();
        }
        
        private void OnSelectedVideoChanged(SelectionChangedMessage<VideoViewModel> message)
        {
            // TODO clear async
            Bookmarks.Clear();
            
            selectedVideo = message.SelectedItem;
            AddTimeBookmarkCommand.RaiseCanExecuteChanged();

            if (selectedVideo == null)
            {
                return;
            }
            
            foreach (var bookmark in selectedVideo.Item.Bookmarks)
            {
                BookmarkViewModel viewModel = bookmark is RangeBookmark rb
                    ? new RangeBookmarkViewModel(rb)
                    : new TimeBookmarkViewModel((bookmark as TimeBookmark)!);
                
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Bookmarks.Add(viewModel);
                });
            }
        }
    }
}
