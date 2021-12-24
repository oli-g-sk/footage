namespace Footage.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Avalonia.Controls;
    using Footage.Repository;
    using Footage.Service;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class MediaSourcesViewModel : ViewModelBase
    {
        private readonly SourcesRepository sourcesRepository;
        
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
            
            var newSource = sourcesRepository.AddLocalSource(directory, true);
            ReloadAllSources();

            // TODO await
            sourcesRepository.RefreshLocalSource(newSource);
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