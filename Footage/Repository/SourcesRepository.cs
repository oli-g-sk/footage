namespace Footage.Repository
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Footage.Dao;
    using Footage.Messages;
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

            using var dao = GetDao();
            await dao.Insert(source);
            await dao.Commit();
            
            return source;
        }

        public async Task RenameSource(int mediaSourceId, string newName)
        {
            if (string.IsNullOrEmpty(newName))
            {
                throw new ArgumentException(nameof(newName));
            }
            
            using var dao = GetDao();
            var source = await dao.Get<MediaSource>(mediaSourceId);
            
            if (source == null)
            {
                // TODO use NotFoundException
                throw new ArgumentException(nameof(mediaSourceId));
            }
            
            source.Name = newName;
            await dao.Update(source);
            await dao.Commit();
            
            MessengerHelper.Send(new EntityRenamedMessage<MediaSource>(source));
        }
        
        public async Task RemoveSource(int mediaSourceId)
        {
            using var dao = GetDao();
            var source = await dao.Get<MediaSource>(mediaSourceId);
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
