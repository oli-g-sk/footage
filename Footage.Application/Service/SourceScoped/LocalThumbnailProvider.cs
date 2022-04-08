namespace Footage.Application.Service.SourceScoped
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

            if (!File.Exists(videoPath))
            {
                Log.Warn($"Cannot generate thumbnail for {video}: Source video file not found.");
                return null;
            }

            try
            {
                // TODO add exception handling
                await ThumbnailMaker.CreateThumbnail(videoPath, outputPath, 320);
                
                var result = Image.FromFile(outputPath);

                File.Delete(outputPath);
                // TODO delete temp files on app shutdown (so it doesn't slow down this?)
                
                return result;
            }
            catch (ThumbnailCreationException ex)
            {
                Log.Error(ex, $"Failed to create thumbnail for {video}. Reason: {ex.InnerException.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error when creating thumbnail for {video}: {ex}");
                return null;
            }
        }
    }
}