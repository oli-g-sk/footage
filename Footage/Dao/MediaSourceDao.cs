namespace Footage.Dao
{
    using System.Collections.Generic;
    using Footage.Context;
    using Footage.Model;
    using JetBrains.Annotations;

    public class MediaSourceDao : DaoBase<MediaSource>
    {
        public MediaSourceDao([NotNull] VideoContext dbContext) : base(dbContext)
        {
        }

        protected override IEnumerable<MediaSource> Entities => DbContext.MediaSources;
    }
}