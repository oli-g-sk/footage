namespace Footage.ViewModel.Section
{
    using System.Linq;
    using System.Threading.Tasks;
    using Footage.Messages;
    using Footage.Model;
    using Footage.Repository;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;

    // TODO support different source types
    // maybe by TInput type being tuple (MediaSourceType, string)
    public class MediaSourcesViewModel : ItemsAddViewModel<MediaSourceViewModel, MediaSource, string?>
    {
        private static SourcesRepository Repo => Locator.Get<SourcesRepository>();

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
            var model = await Repo.AddLocalSource(input, true);

            // TODO move source files refresh to selection changed handler?
            await Repo.ImportNewFiles(model);
            
            return model;
        }
        
        protected override MediaSourceViewModel CreateViewModel(MediaSource model) => new(model);

        protected override async Task DeleteModel(MediaSource item)
        {
            await Repo.RemoveSource(item);
        }

        protected override void AfterSelectionChanged()
        {
            base.AfterSelectionChanged();

            // TODO await
            if (SelectedItem?.Item is LocalMediaSource localSource)
            {
                Repo.ImportNewFiles(localSource);
            }
        }

        private async Task LoadAllSources()
        {
            var sources = (await Repo.GetAllSources()).Select(s => new MediaSourceViewModel(s));
            
            // TODO create an extension method for this
            foreach (var source in sources)
            {
                Items.Add(source);
            }
        }
    }
}