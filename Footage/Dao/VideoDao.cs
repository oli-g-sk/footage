namespace Footage.Dao
{
    using System.Linq;
    using Footage.Context;
    using Footage.Model;

    public class VideoDao : DaoBase<Video>, IVideoDao
    {
        protected override IQueryable<Video> GetEntities(VideoContext context)
        {
            return context.Videos;
        }
    }
}