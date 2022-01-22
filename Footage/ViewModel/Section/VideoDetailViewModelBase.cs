namespace Footage.ViewModel.Section
{
    using Footage.Messages;
    using Footage.Repository;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;
    using System;

    public abstract class VideoDetailViewModelBase : SectionViewModel
    {
        protected MediaSourceViewModel? SelectedMediaSource { get; private set; }

        private VideoViewModel? selectedVideo; 
        protected VideoViewModel? SelectedVideo
        {
            get => selectedVideo;
            private set => Set(ref selectedVideo, value);
        }

        protected VideoDetailViewModelBase()
        {
            MessengerInstance.Register<SelectionChangedMessage<VideoViewModel>>(this, OnSelectedVideoChanged);
            MessengerInstance.Register<SelectionChangedMessage<MediaSourceViewModel>>(this, OnMediaSourceChanged);
        }

        protected virtual void BeforeSelectedVideoChanged()
        {
        }

        protected virtual void AfterSelectedVideoChanged()
        {
        }

        protected virtual void BeforeMediaSourceChanged()
        {
        }

        protected virtual void AfterMediaSourceChanged()
        {
        }

        private void OnSelectedVideoChanged(SelectionChangedMessage<VideoViewModel> message)
        {
            if (message.SelectedItem != null)
            {
                BeforeSelectedVideoChanged();
                SelectedVideo = message.SelectedItem;
                AfterSelectedVideoChanged();
            }
        }

        private void OnMediaSourceChanged(SelectionChangedMessage<MediaSourceViewModel> message)
        {
            BeforeMediaSourceChanged();
            SelectedMediaSource = message.SelectedItem;
            AfterMediaSourceChanged();
        }
    }
}