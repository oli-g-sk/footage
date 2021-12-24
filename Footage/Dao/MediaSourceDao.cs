namespace Footage.Dao
{
    using System.Collections.Generic;
    using System.Linq;
    using Footage.Context;
    using Footage.Model;

    public class MediaSourceDao : DaoBase<MediaSource>, IMediaSourceDao
    {
        protected override IQueryable<MediaSource> GetEntities(VideoContext context)
        {
            return context.MediaSources;
        }
    }
}