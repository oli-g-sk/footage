namespace Footage.ViewModel.Section
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Avalonia.Controls;
    using Avalonia.Threading;
    using Footage.Messages;
    using Footage.Repository;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;
    using GalaSoft.MvvmLight.Command;

    public class MediaSourcesViewModel : SectionViewModel
    {
        public ObservableCollection<MediaSourceViewModel> Sources { get; }

        private MediaSourceViewModel? selectedSource;

        public MediaSourceViewModel? SelectedSource
        {
            get => selectedSource;
            set
            {
                Set(ref selectedSource, value);
                RaisePropertyChanged(nameof(SourceSelected));
                RemoveSelectedSourceCommand.RaiseCanExecuteChanged();
                MessengerInstance.Send(new SelectionChangedMessage<MediaSourceViewModel>(SelectedSource));
            }
        }

        public bool SourceSelected => SelectedSource != null;
        
        public RelayCommand AddLocalSourceCommand { get; }
        
        public RelayCommand RemoveSelectedSourceCommand { get; }

        public MediaSourcesViewModel()
        {
            Sources = new ObservableCollection<MediaSourceViewModel>();
            AddLocalSourceCommand = new RelayCommand(AddLocalSource);
            RemoveSelectedSourceCommand = new RelayCommand(RemoveSelectedSource, () => SourceSelected);
            
            LoadAllSources();
        }

        private void AddLocalSource()
        {
            // TODO move to View layer
            var dialog = new OpenFolderDialog();
            
            // TODO make async
            var task = dialog.ShowAsync(MainWindow.Instance);
            task.Wait();
            string? directory = task.Result;

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
                Sources.Add(source);
            }
        }
    }
}