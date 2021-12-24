namespace Footage.ViewModel
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using Avalonia;
    using Avalonia.Controls;
    using Footage.Context;
    using Footage.Dao;
    using Footage.Model;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class MediaSourcesViewModel : ViewModelBase
    {
        private IEntityDao<MediaSource> Dao { get; }

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

        public MediaSourcesViewModel()
        {
            // TODO inject
            var dbContext = new VideoContext();
            Dao = new MediaSourceDao(dbContext);

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
            
            // TODO move to Repository layer
            var source = new LocalMediaSource
            {
                Name = Path.GetFileName(directory),
                RootPath = directory,
                IncludeSubfolders = true
            };
                
            Dao.Insert(source);
            
            ReloadAllSources();
        }

        private void RemoveSelectedSource()
        {
            Dao.Remove(SelectedSource.Item);
            ReloadAllSources();
        }
        
        private void ReloadAllSources()
        {
            Sources.Clear();
            var sources = Dao.Query().Select(s => new MediaSourceViewModel(s));
            
            // TODO create an extension method for this
            foreach (var source in sources)
            {
                Sources.Add(source);
            }
        }
    }
}