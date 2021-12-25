﻿namespace Footage.ViewModel.Entity
{
    using Footage.Model;
    using Footage.ViewModel.Base;
    using JetBrains.Annotations;

    public class MediaSourceViewModel : EntityViewModel<MediaSource>
    {
        public string Name => Item.Name;

        // TODO include a second description line for other (future) source types
        public string Description =>
            (Item is LocalMediaSource localMediaSource) ? localMediaSource.RootPath : "Unknown source";

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => Set(ref isBusy, value);
        }

        public bool IsActive
        {
            get => Item.Active;
            set
            {
                Item.Active = value;
                RaisePropertyChanged(nameof(IsActive));
            }
        }
        
        public MediaSourceViewModel([NotNull] MediaSource item) : base(item)
        {
        }
    }
}