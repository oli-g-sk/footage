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

        public ObservableCollection<EntityViewModel<MediaSource>> Sources { get; }
        
        public RelayCommand AddLocalSourceCommand { get; }

        public MediaSourcesViewModel()
        {
            // TODO inject
            var dbContext = new VideoContext();
            Dao = new MediaSourceDao(dbContext);

            Sources = new ObservableCollection<EntityViewModel<MediaSource>>();
            AddLocalSourceCommand = new RelayCommand(AddLocalSource);
            
            ReloadAllSources();
        }

        private void AddLocalSource()
        {
            // TODO move to View layer
            var dialog = new OpenFolderDialog();
            
            // TODO make async
            var task = dialog.ShowAsync(MainWindow.Instance);
            task.Wait();
            
            if (dialog.Directory != null)
            {
                var source = new LocalMediaSource
                {
                    Name = Path.GetDirectoryName(dialog.Directory),
                    IncludeSubfolders = true,
                };
                
                Dao.Insert(source);
                
                ReloadAllSources();
            }
        }

        private void ReloadAllSources()
        {
            Sources.Clear();
            var sources = Dao.Query().Select(s => new EntityViewModel<MediaSource>(s));
            
            // TODO create an extension method for this
            foreach (var source in sources)
            {
                Sources.Add(source);
            }
        }
    }
}