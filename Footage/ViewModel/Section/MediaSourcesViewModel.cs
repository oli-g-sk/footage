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

        public bool InteractionEnabled => !AnySourceUpdating && !SelectedSourceLoading;

        private bool anySourceUpdating;
        public bool AnySourceUpdating
        {
            get => anySourceUpdating;
            set
            {
                Set(ref anySourceUpdating, value);
                RaisePropertyChanged(nameof(InteractionEnabled));
                RemoveSelectedItemCommand.RaiseCanExecuteChanged();
            } 
        }

        private bool selectedSourceLoading;

        public bool SelectedSourceLoading
        {
            get => selectedSourceLoading;
            set
            {
                Set(ref selectedSourceLoading, value);
                RaisePropertyChanged(nameof(InteractionEnabled));
                RemoveSelectedItemCommand.RaiseCanExecuteChanged();
            }
        }

        public MediaSourcesViewModel()
        {
            Task.Run(LoadAllSources);

            MessengerInstance.Register<IsBusyChangedMessage>(this, m => SelectedSourceLoading = m.IsBusy);
        }

        protected override async Task OnItemAdded(MediaSourceViewModel viewModel)
        {
            await UpdateSource(viewModel);
            SelectedItem = viewModel;
        }

        protected override bool CanAddItem(string? item)
        {
            return !string.IsNullOrEmpty(item);
        }

        protected override bool CanRemoveSelectedItem()
        {
            return base.CanRemoveSelectedItem() && InteractionEnabled;
        }

        protected override async Task<MediaSource> CreateAndStoreModel(string? input)
        {
            return await SourceRepo.AddLocalSource(input, true);
        }
        
        protected override MediaSourceViewModel CreateViewModel(MediaSource model) => new(model);

        protected override async Task DeleteModel(MediaSource item)
        {
            await SourceRepo.RemoveSource(item.Id);
        }

        private async Task LoadAllSources()
        {
            var sources = (await SourceRepo.GetAllSources()).Select(s => new MediaSourceViewModel(s));
            
            // TODO create an extension method for this
            foreach (var source in sources)
            {
                Items.Add(source);
            }
            
            await UpdateAllSources();
        }

        private async Task UpdateAllSources()
        {
            await Task.WhenAll(Items.Select(UpdateSource));
        }

        private async Task UpdateSource(MediaSourceViewModel source)
        {
            var sourceId = source.Item.Id;
            
            await Dispatcher.InvokeAsync(() =>
            {
                AnySourceUpdating = true;
                source.IsBusy = true;
            });

            await LibraryRepo.ImportNewFiles(sourceId);
            
            source.VideoCount = await LibraryRepo.GetVideoCount(sourceId); 
            
            await Dispatcher.InvokeAsync(() =>
            {
                source.IsBusy = false;
                AnySourceUpdating = false;
            });
        }
    }
}