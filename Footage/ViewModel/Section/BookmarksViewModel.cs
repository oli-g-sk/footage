namespace Footage.ViewModel.Section
{
    using System;
    using System.Collections.ObjectModel;
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
        
        public RelayCommand<long> AddTimeBookmarkCommand { get; }
        
        public BookmarksViewModel()
        {
            Bookmarks = new ObservableCollection<BookmarkViewModel>();
            AddTimeBookmarkCommand = new RelayCommand<long>(AddTimeBookmark, _ => selectedVideo != null);
            MessengerInstance.Register<SelectionChangedMessage<VideoViewModel>>(this, OnSelectedVideoChanged);
        }

        // TODO make async
        public void AddTimeBookmark(long timestamp)
        {
            using var repo = new BookmarksRepository();
            // TODO await
            var task = repo.AddTimeBookmarkToVideo(selectedVideo.Item, timestamp);
            task.Wait();
            var bookmark = task.Result;
            Bookmarks.Add(new TimeBookmarkViewModel(bookmark));
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
