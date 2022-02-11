namespace Footage.Repository
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Footage.Dao;
    using Footage.Model;
    using Footage.Service;
    using Microsoft.EntityFrameworkCore;

    public class SourcesRepository : RepositoryBase
    {
        public async Task<LocalMediaSource> AddLocalSource(string path, bool includeSubfolders)
        {
            var source = new LocalMediaSource
            {
                RootPath = path,
                IncludeSubfolders = includeSubfolders,
                Name = Path.GetFileName(path)
            };
            
            return source;
        }

        public async Task RemoveSource(MediaSource source)
        {
            using var dao = GetDao();
            await dao.Remove(source);
            await dao.Commit();
        }

        public async Task<IEnumerable<MediaSource>> GetAllSources()
        {
            using var dao = GetDao();
            return await dao.Query<MediaSource>().ToListAsync();
        }
    }
}