namespace Footage.Engine
{
    using System.Drawing;
    using System.Threading.Tasks;

    public interface IThumbnailMaker
    {
        Task<Bitmap> CreateThumbnail(string mediaPath, int width, int time);
    }
}
