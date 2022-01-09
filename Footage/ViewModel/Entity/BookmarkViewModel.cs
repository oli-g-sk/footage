﻿namespace Footage.ViewModel.Entity
{
    using Footage.Model;
    using Footage.ViewModel.Base;
    using GalaSoft.MvvmLight;

    public abstract class BookmarkViewModel : EntityViewModel<Bookmark>
    {
        public bool IsRange => Item is RangeBookmark;

        public long VideoDuration => Item.Video.Duration;

        public BookmarkViewModel(Bookmark bookmark) : base(bookmark)
        {
        }
    }
}