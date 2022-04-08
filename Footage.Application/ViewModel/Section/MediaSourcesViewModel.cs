namespace Footage.Application.ViewModel.Section
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Footage.Application.Messages;
    using Footage.Application.Repository;
    using Footage.Application.Service;
    using Footage.Application.ViewModel.Base;
    using Footage.Application.ViewModel.Entity;
    using Footage.Model;
    using GalaSoft.MvvmLight.Command;

    // TODO support different source types
    // maybe by TInput type being tuple (MediaSourceType, string)
    public class MediaSourcesViewModel : ItemsAddViewModel<MediaSourceViewModel, MediaSource>
    {
        private static IDialogService DialogService => Locator.Get<IDialogService>();
        
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
                
                AddItemCommand.RaiseCanExecuteChanged();
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
                
                AddItemCommand.RaiseCanExecuteChanged();
                RemoveSelectedItemCommand.RaiseCanExecuteChanged();
            }
        }
        
        public RelayCommand RenameSelectedItemCommand { get; private set; }

        public MediaSourcesViewModel()
        {
            RenameSelectedItemCommand = new RelayCommand(RenameSelectedItem, CanRenameSelectedItem);
            MessengerInstance.Register<IsBusyChangedMessage>(this, m => SelectedSourceLoading = m.IsBusy);
        }

        protected override async Task OnItemAdded(MediaSourceViewModel viewModel)
        {
            await UpdateSource(viewModel);
            SelectedItem = viewModel;
        }

        protected override bool CanAddItem()
        {
            return base.CanAddItem() && InteractionEnabled;
        }

        protected override bool CanRemoveSelectedItem()
        {
            return base.CanRemoveSelectedItem() && InteractionEnabled;
        }

        protected override async Task<bool> IsItemRemoveConfirmed(MediaSource item)
        {
            var dialogService = Locator.Get<IDialogService>();
            
            // l18n
            return await dialogService.ShowYesNo("Delete media source",
                "Are you sure you want to delete the selected media source?");
        }

        protected override async Task<MediaSource?> CreateAndStoreModel()
        {
            var dialogService = Locator.Get<IDialogService>();
            string? path = await dialogService.SelectFolder();

            if (path == null)
            {
                return null;
            }
            
            return await SourceRepo.AddLocalSource(path, true);
        }
        
        protected override MediaSourceViewModel CreateViewModel(MediaSource model) => new(model);

        protected override async Task DeleteModel(MediaSource item)
        {
            await SourceRepo.RemoveSource(item.Id);
        }

        protected override async Task<IEnumerable<MediaSource>> LoadAllItems()
        {
            return await SourceRepo.GetAllSources();
        }

        protected override async Task OnItemsLoaded()
        {
            await UpdateAllSources();
        }

        protected override void AfterSelectionChanged()
        {
            base.AfterSelectionChanged();
            RenameSelectedItemCommand.RaiseCanExecuteChanged();
        }

        // TODO use task
        // TODO REFACTOR duplicity with ProjectsViewModel
        private async void RenameSelectedItem()
        {
            if (SelectedItem == null)
            {
                return;
            }
            
            var result = await DialogService.ShowInput("Rename media source", "Enter a new name for this media source", SelectedItem.Name);

            if (result.Confirmed && !string.IsNullOrEmpty(result.InputValue))
            {
                await SourceRepo.RenameSource(SelectedItem.Id, result.InputValue);
            }
        }

        private bool CanRenameSelectedItem() => SelectedItem != null;

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