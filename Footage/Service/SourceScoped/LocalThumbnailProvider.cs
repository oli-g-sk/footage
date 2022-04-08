namespace Footage.Service.SourceScoped
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Threading.Tasks;
    using Footage.Engine;
    using Footage.Model;
    using NLog;

    public class LocalThumbnailProvider : SourceScopedServiceBase, IThumbnailProvider
    {
        private static ILogger Log => LogManager.GetCurrentClassLogger();
        
        private static IThumbnailMaker ThumbnailMaker => Locator.Get<IThumbnailMaker>();

        private readonly LocalMediaProviderService localMediaProviderService;
        
        public LocalThumbnailProvider(LocalMediaProviderService localMediaProviderService, LocalMediaSource source) : base(source)
        {
            this.localMediaProviderService = localMediaProviderService ?? 
                throw new ArgumentNullException(nameof(localMediaProviderService));
        }

        public async Task<Image?> GetDefaultThumbnail(Video video)
        {
            string tempName = Guid.NewGuid().ToString();
            string videoPath = localMediaProviderService.GetFullPath(video);
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), $"{tempName}.jpg");

            try
            {
                await ThumbnailMaker.CreateThumbnail(videoPath, outputPath, 320);
                // TODO add exception handling
                return Image.FromFile(outputPath);
            }
            catch (ThumbnailCreationException ex)
            {
                Log.Error(ex, $"Failed to create thumbnail for {video}. Reason: {ex.InnerException.Message}");
                return null;
            }
        }
    }
}