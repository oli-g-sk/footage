namespace Footage.ViewModel.Section
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Avalonia.Controls;
    using Avalonia.Threading;
    using Footage.Repository;
    using Footage.ViewModel.Entity;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class MediaSourcesViewModel : ViewModelBase
    {
        private readonly SourcesRepository sourcesRepository;

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

        public MediaSourcesViewModel(SourcesRepository sourcesRepository)
        {
            this.sourcesRepository = sourcesRepository;

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
                await Task.Delay(1);
                var newSource = await sourcesRepository.AddLocalSource(directory, true);

                var viewModel = new MediaSourceViewModel(newSource) { IsBusy = true };

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Sources.Add(viewModel);
                });

                await sourcesRepository.RefreshLocalSource(newSource).ContinueWith(t => viewModel.IsBusy = false);
            }));
        }

        private void RemoveSelectedSource()
        {
            sourcesRepository.RemoveSource(SelectedSource.Item);
            ReloadAllSources();
        }
        
        private void ReloadAllSources()
        {
            Sources.Clear();
            var sources = sourcesRepository.Sources.Select(s => new MediaSourceViewModel(s));
            
            // TODO create an extension method for this
            foreach (var source in sources)
            {
                Sources.Add(source);
            }
        }
    }
}