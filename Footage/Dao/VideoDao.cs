namespace Footage.Dao
{
    using System.Collections.Generic;
    using Footage.Context;
    using Footage.Model;
    using JetBrains.Annotations;

    public class VideoDao : DaoBase<Video>
    {
        public VideoDao([NotNull] VideoContext dbContext) : base(dbContext)
        {
        }

        protected override IEnumerable<Video>? Entities => DbContext.Videos;
    }
}