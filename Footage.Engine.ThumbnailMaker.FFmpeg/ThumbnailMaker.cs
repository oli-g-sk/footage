namespace Footage.Engine.ThumbnailMaker.FFmpeg
{
    using System.Threading.Tasks;
    using Okisioli.FFtool;
    using FFThumbnailMaker = Okisioli.FFtool.ThumbnailMaker;

    public class ThumbnailMaker : IThumbnailMaker
    {
        public async Task CreateThumbnail(string mediaPath, string outputPath, int width)
        {
            var thumbnailMaker = new FFThumbnailMaker(outputPath, ThumbnailFormat.Jpeg);
            await thumbnailMaker.OfWidth(width, mediaPath);
        }
    }
}
