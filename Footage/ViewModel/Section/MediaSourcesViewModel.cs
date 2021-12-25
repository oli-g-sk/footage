namespace Footage.ViewModel.Section
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Avalonia.Controls;
    using Avalonia.Threading;
    using Footage.Dao;
    using Footage.Repository;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class MediaSourcesViewModel : SectionViewModel
    {
        public event EventHandler SelectedSourceChanged;
        
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
                SelectedSourceChanged?.Invoke(this, EventArgs.Empty);
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
            
            ReloadAllSources();
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

            Task.Run(new Action(async () =>
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
            }));
        }

        private void RemoveSelectedSource()
        {
            using var repo = Locator.Get<SourcesRepository>();
            repo.RemoveSource(SelectedSource.Item);
            ReloadAllSources();
        }
        
        private void ReloadAllSources()
        {
            Sources.Clear();

            using var repo = Locator.Get<SourcesRepository>();
            var sources = repo.Sources.Select(s => new MediaSourceViewModel(s));
            
            // TODO create an extension method for this
            foreach (var source in sources)
            {
                Sources.Add(source);
            }
        }
    }
}