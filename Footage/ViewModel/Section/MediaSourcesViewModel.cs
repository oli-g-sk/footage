namespace Footage.ViewModel.Section
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Avalonia.Controls;
    using Avalonia.Threading;
    using Footage.Messages;
    using Footage.Model;
    using Footage.Repository;
    using Footage.Service;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;
    using GalaSoft.MvvmLight.Command;

    // TODO support different source types
    // maybe by TInput type being tuple (MediaSourceType, string)
    public class MediaSourcesViewModel : ItemsViewModel<MediaSourceViewModel, MediaSource, string?>
    {
        private bool selectionEnabled;

        public bool SelectionEnabled
        {
            get => selectionEnabled;
            set => Set(ref selectionEnabled, value);
        }

        public MediaSourcesViewModel()
        {
            Task.Run(LoadAllSources)
                .ContinueWith(_ => SelectionEnabled = true);
            
            MessengerInstance.Register<IsBusyChangedMessage>(this, m => SelectionEnabled = !m.IsBusy);
        }
        
        protected override bool CanAddItem(string? item)
        {
            return !string.IsNullOrEmpty(item);
        }
        
        protected override async Task<MediaSource> CreateAndStoreModel(string? input)
        {
            using var repo = new SourcesRepository();
            var model = await repo.AddLocalSource(input, true);
            return model;
        }
        
        protected override MediaSourceViewModel CreateViewModel(MediaSource model) => new(model);

        protected override async Task DeleteModel(MediaSource item)
        {
            using var repo = new SourcesRepository();
            await repo.RemoveSource(item);
        }

        protected override void AfterSelectionChanged()
        {
            base.AfterSelectionChanged();
            MessengerInstance.Send(new SelectedMesiaSourceChangedMessage(SelectedItem, GetMediaProvider(SelectedItem)));
        }
        
        private async Task LoadAllSources()
        {
            using var repo = Locator.Get<SourcesRepository>();
            var sources = (await repo.GetAllSources()).Select(s => new MediaSourceViewModel(s));
            
            // TODO create an extension method for this
            foreach (var source in sources)
            {
                Items.Add(source);
            }
        }
        
        private static MediaProviderBase? GetMediaProvider(MediaSourceViewModel? source)
        {
            if (source == null)
            {
                return null;
            }
            
            if (source.Item is LocalMediaSource localSource)
            {
                return new LocalMediaProvider(localSource);
            }

            // TODO create different provider for different source type
            throw new NotImplementedException();
        }
        
        /*

        private void AddLocalSource()
        {


            if (directory == null)
            {
                return;
            }

            Task.Run(async () =>
            {
                using var repo = Locator.Get<SourcesRepository>();
                
                await Task.Delay(1);
                var newSource = await repo.AddLocalSource(directory, true);

                var viewModel = new MediaSourceViewModel(newSource) { IsBusy = true };

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Sources.Add(viewModel);
                });

                await repo.RefreshLocalSource(newSource).ContinueWith(t => viewModel.IsBusy = false);
            });
        }

        private void RemoveSelectedSource()
        {
            if (SelectedSource == null)
            {
                return;
            }
            
            var sourceModel = SelectedSource.Item;
            Sources.Remove(SelectedSource);
            
            Task.Run(async () =>
            {
                using var repo = Locator.Get<SourcesRepository>();
                await repo.RemoveSource(sourceModel);
            });
        }
        
        private async Task LoadAllSources()
        {
            using var repo = Locator.Get<SourcesRepository>();
            var sources = (await repo.GetAllSources()).Select(s => new MediaSourceViewModel(s));
            
            // TODO create an extension method for this
            foreach (var source in sources)
            {
                Items.Add(source);
            }
        }
        
        */
    }
}