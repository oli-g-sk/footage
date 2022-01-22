﻿namespace Footage.ViewModel.Section
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Footage.Messages;
    using Footage.Model;
    using Footage.Repository;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;

    public class VideoBrowserViewModel : ItemsViewModel<VideoViewModel, Video>
    {
        private readonly VideoBrowserRepository repo = new VideoBrowserRepository();

        private MediaSource? selectedSource;

        public VideoBrowserViewModel()
        {
            MessengerInstance.Register<SelectionChangedMessage<MediaSourceViewModel>>(this, OnMediaSourceChanged);
        }

        private void OnMediaSourceChanged(SelectionChangedMessage<MediaSourceViewModel> message)
        {
            selectedSource = message.SelectedItem?.Item;
            
            // TODO clear async
            Items.Clear();
            
            // TODO await?
            FetchVideos();
        }

        private async Task FetchVideos(int? batchSize = null)
        {
            if (selectedSource == null)
            {
                return;
            }
            
            MessengerInstance.Send(new IsBusyChangedMessage(true));
            
            var videos = await repo.FetchVideos(selectedSource, batchSize);

            foreach (var video in videos)
            {
                await Dispatcher.InvokeAsync(async () =>
                {
                    Items.Add(new VideoViewModel(video));
                });

#if DEBUG
                await Task.Delay(125);
#endif
            }
            
            MessengerInstance.Send(new IsBusyChangedMessage(false));
        }

        protected override Task DeleteModel(Video item)
        {
            throw new NotImplementedException();
        }
    }
}
