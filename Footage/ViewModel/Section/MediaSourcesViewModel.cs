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
    public class MediaSourcesViewModel : ItemsAddViewModel<MediaSourceViewModel, MediaSource, string?>
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

            // TODO move source files refresh to selection changed handler?
            await repo.ImportNewFiles(model);
            
            return model;
        }
        
        protected override MediaSourceViewModel CreateViewModel(MediaSource model) => new(model);

        protected override async Task DeleteModel(MediaSource item)
        {
            using var repo = new SourcesRepository();
            await repo.RemoveSource(item);
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
    }
}