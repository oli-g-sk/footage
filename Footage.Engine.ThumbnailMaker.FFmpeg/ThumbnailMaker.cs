namespace Footage.Engine.ThumbnailMaker.FFmpeg
{
    using System.Threading.Tasks;
    using Okisioli.FFtool;
    using FFThumbnailMaker = Okisioli.FFtool.ThumbnailMaker;

    public class ThumbnailMaker : IThumbnailMaker
    {
        static ThumbnailMaker()
        {
            FFTool.Initialize("ffmpeg.exe");
        }
        
        public async Task CreateThumbnail(string mediaPath, string outputPath, int width)
        {
            try
            {
                var thumbnailMaker = new FFThumbnailMaker(mediaPath, ThumbnailFormat.Jpeg);
                
                // TODO make sure FFtool library throws descriptive FFmpeg errors
                await thumbnailMaker.OfWidth(width, outputPath);
            }
            catch (Exception ex)
            {
                throw new ThumbnailCreationException(ex);
            }
        }
    }
}
