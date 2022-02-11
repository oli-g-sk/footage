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
        private static SourcesRepository SourceRepo => Locator.Get<SourcesRepository>();
        private static LibraryRepository LibraryRepo => Locator.Get<LibraryRepository>();

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
            return await SourceRepo.AddLocalSource(input, true);
        }
        
        protected override MediaSourceViewModel CreateViewModel(MediaSource model) => new(model);

        protected override async Task DeleteModel(MediaSource item)
        {
            await SourceRepo.RemoveSource(item);
        }

        protected override void AfterSelectionChanged()
        {
            base.AfterSelectionChanged();

            if (SelectedItem?.Item is LocalMediaSource localSource)
            {
                // TODO await
                LibraryRepo.ImportNewFiles(localSource);
            }
        }

        private async Task LoadAllSources()
        {
            var sources = (await SourceRepo.GetAllSources()).Select(s => new MediaSourceViewModel(s));
            
            // TODO create an extension method for this
            foreach (var source in sources)
            {
                Items.Add(source);
            }
        }
    }
}