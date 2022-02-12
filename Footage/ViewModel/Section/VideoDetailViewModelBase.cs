namespace Footage.ViewModel.Section
{
    using Footage.Messages;
    using Footage.Repository;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;
    using System;
    using Footage.Model;

    public abstract class VideoDetailViewModelBase : SectionViewModel
    {
        protected MediaSourceViewModel? SelectedMediaSource { get; private set; }

        private VideoViewModel? selectedVideo; 
        protected VideoViewModel? SelectedVideo
        {
            get => selectedVideo;
            private set
            {
                BeforeSelectedVideoChanged();
                Set(ref selectedVideo, value);
                AfterSelectedVideoChanged();
            }
        }

        protected VideoDetailViewModelBase()
        {
            MessengerInstance.Register<SelectionChangedMessage<VideoViewModel>>(this, OnSelectedVideoChanged);
            MessengerInstance.Register<SelectionChangedMessage<MediaSourceViewModel>>(this, OnMediaSourceChanged);
            MessengerInstance.Register<EntityDeletedMessage<MediaSource>>(this, OnMediaSourceDeleted);
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
                SelectedVideo = message.SelectedItem;
            }
        }

        private void OnMediaSourceChanged(SelectionChangedMessage<MediaSourceViewModel> message)
        {
            BeforeMediaSourceChanged();
            SelectedMediaSource = message.SelectedItem;
            AfterMediaSourceChanged();
        }
        
        private void OnMediaSourceDeleted(EntityDeletedMessage<MediaSource> msg)
        {
            // if we deleted source containing the currently selected video, unload the video
            if (SelectedVideo != null && msg.DeletedEntity.Equals(SelectedVideo.Item.MediaSource))
            {
                SelectedVideo = null;
            }
        }
    }
}