﻿namespace Footage.ViewModel.Section
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Avalonia.Threading;
    using Footage.Messages;
    using Footage.Model;
    using Footage.Repository;
    using Footage.Service;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;

    public class VideoBrowserViewModel : ItemsViewModel<VideoViewModel, Video>
    {
        private MediaSource? selectedSource;

        public VideoBrowserViewModel()
        {
            MessengerInstance.Register<SelectionChangedMessage<MediaSource>>(this, OnMediaSourceChanged);
        }

        private void OnMediaSourceChanged(SelectionChangedMessage<MediaSource> message)
        {
            selectedSource = message.SelectedItem;
            
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
            
            using var repo = new VideoBrowserRepository();
            
            var videos = repo.FetchVideos(selectedSource, batchSize).ToList();

            foreach (var video in videos)
            {
                await Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    await repo.UpdateVideoDuration(selectedSource, video);
                    Items.Add(new VideoViewModel(video));
                });

#if DEBUG
                // await Task.Delay(25);
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
