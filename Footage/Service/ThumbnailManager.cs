namespace Footage.Service
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Threading.Tasks;
    using Footage.Model;
    using Footage.Service.SourceScoped;

    public class ThumbnailManager : IThumbnailManager
    {
        private static string ThumbnailFolder => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Footage", "thumbs");

        private readonly ISourceScopedServiceFactory sourceScopedServiceFactory;

        static ThumbnailManager()
        {
            if (!Directory.Exists(ThumbnailFolder))
                Directory.CreateDirectory(ThumbnailFolder);
        }

        public ThumbnailManager(ISourceScopedServiceFactory sourceScopedServiceFactory)
        {
            this.sourceScopedServiceFactory = sourceScopedServiceFactory ??
                throw new ArgumentNullException(nameof(sourceScopedServiceFactory));
        }

        public async Task<string?> GetThumbnail(Video video)
        {
            if (!HasCachedThumbnail(video))
            {
                var image = await CreateDefaultThumbnail(video);
                SaveThumbnail(video, image);
            }

            // TODO add exception handling, check read access
            string thumbPath = GetThumbPath(video);
            return thumbPath;
        }
        
        private static bool HasCachedThumbnail(Video video)
        {
            return File.Exists(GetThumbPath(video));
        }
        
        private async Task<Image> CreateDefaultThumbnail(Video video)
        {
            var thumbProvider = GetThumbnailProvider(video);
            return await thumbProvider.GetDefaultThumbnail(video);
        }

        private static void SaveThumbnail(Video video, Image thumbnail)
        {
            // TODO add exception handling
            thumbnail.Save(GetThumbPath(video));
        }

        private static string GetThumbPath(Video video) => Path.Combine(ThumbnailFolder, $"video_{video.Id}.jpg");

        private IThumbnailProvider GetThumbnailProvider(Video video)
            => sourceScopedServiceFactory.GetThumbnailProviderService(video.MediaSource);
    }
}