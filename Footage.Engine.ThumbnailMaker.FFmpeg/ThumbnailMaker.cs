namespace Footage.Engine.ThumbnailMaker.FFmpeg
{
    using System.Drawing;
    using System.Threading.Tasks;

    public class ThumbnailMaker : IThumbnailMaker
    {
        public Task<Bitmap> CreateThumbnail(string mediaPath, int width, int time)
        {
            throw new System.NotImplementedException();
        }
    }
}
