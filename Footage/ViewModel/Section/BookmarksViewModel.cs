namespace Footage.ViewModel.Section
{
    using System;
    using System.Collections.ObjectModel;
    using Avalonia.Threading;
    using Footage.Model;
    using Footage.Repository;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;
    using GalaSoft.MvvmLight.Command;

    public class BookmarksViewModel : SectionViewModel
    {
        private readonly SelectedVideoViewModel selectedVideoViewModel;

        private VideoViewModel? SelectedVideo => selectedVideoViewModel.SelectedVideo;
        
        public ObservableCollection<BookmarkViewModel> Bookmarks { get; }
        
        public RelayCommand<long> AddTimeBookmarkCommand { get; }
        
        public BookmarksViewModel(SelectedVideoViewModel selectedVideoViewModel)
        {
            this.selectedVideoViewModel = selectedVideoViewModel;
            Bookmarks = new ObservableCollection<BookmarkViewModel>();

            AddTimeBookmarkCommand = new RelayCommand<long>(AddTimeBookmark, _ => SelectedVideo != null);
            this.selectedVideoViewModel.BeforeVideoChanged += SelectedVideoViewModel_BeforeVideoChanged;
            this.selectedVideoViewModel.AfterVideoChanged += SelectedVideoViewModel_AfterVideoChanged;
        }

        // TODO make async
        public void AddTimeBookmark(long timestamp)
        {
            var bookmark = new TimeBookmark();
            
            bookmark.Time = timestamp;
            bookmark.Video = SelectedVideo.Item;
            using var repo = new BookmarksRepository();
            repo.AddBookmark(bookmark).Wait();
        }

        private void SelectedVideoViewModel_BeforeVideoChanged(object? sender, EventArgs e)
        {
            // TODO clear async
            Bookmarks.Clear();
        }
        
        private void SelectedVideoViewModel_AfterVideoChanged(object? sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(SelectedVideo));
            AddTimeBookmarkCommand.RaiseCanExecuteChanged();

            if (SelectedVideo == null)
            {
                return;
            }
            
            foreach (var bookmark in SelectedVideo.Item.Bookmarks)
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