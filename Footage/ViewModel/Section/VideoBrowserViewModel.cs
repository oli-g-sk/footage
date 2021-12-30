namespace Footage.ViewModel.Section
{
    using System;
    using System.Collections.ObjectModel;
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
            MessengerInstance.Register<SelectedMesiaSourceChangedMessage>(this, m => SwitchSource(m.SelectedItem));
        }

        protected override void AfterSelectionChanged()
        {
            base.AfterSelectionChanged();
            MessengerInstance.Send(new SelectionChangedMessage<VideoViewModel>(SelectedItem));
        }

        private async Task SwitchSource(MediaSourceViewModel? source)
        {
            selectedSource = source?.Item;
            
            // TODO clear async
            Items.Clear();
            
            await FetchVideos();
        }

        private async Task FetchVideos(int? batchSize = null)
        {
            if (selectedSource == null)
            {
                return;
            }
            
            MessengerInstance.Send(new IsBusyChangedMessage(true));
            
            using var repo = new VideoBrowserRepository();
            
            var videos = repo.FetchVideos(selectedSource, batchSize);

            foreach (var video in videos)
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Items.Add(new VideoViewModel(video));
                });

#if DEBUG
                await Task.Delay(25);
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
