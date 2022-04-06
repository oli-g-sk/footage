namespace Footage.Service.SourceScoped
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Threading.Tasks;
    using Footage.Engine;
    using Footage.Model;

    public class LocalThumbnailProvider : SourceScopedServiceBase, IThumbnailProvider
    {
        private static IThumbnailMaker ThumbnailMaker => Locator.Get<IThumbnailMaker>();

        private readonly LocalMediaProviderService localMediaProviderService;
        
        public LocalThumbnailProvider(LocalMediaProviderService localMediaProviderService, LocalMediaSource source) : base(source)
        {
            this.localMediaProviderService = localMediaProviderService ?? 
                throw new ArgumentNullException(nameof(localMediaProviderService));
        }

        public async Task<Image> GetDefaultThumbnail(Video video)
        {
            string tempName = new Guid().ToString();
            string videoPath = localMediaProviderService.GetFullPath(video);
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), $"{tempName}.jpg");
            
            // TODO handle FFmpeg errors
            await ThumbnailMaker.CreateThumbnail(videoPath, outputPath, 320);
            
            // TODO add exception handling
            return Image.FromFile(outputPath);
        }
    }
}