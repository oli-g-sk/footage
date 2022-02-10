﻿namespace Footage.Repository
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

            await ImportNewFiles(source);
            
            return source;
        }

        public async Task RemoveSource(MediaSource source)
        {
            using var dao = GetDao();
            await dao.Remove(source);
            await dao.Commit();
        }

        public async Task ImportNewFiles(LocalMediaSource source)
        {
            var provider = new LocalMediaProvider(source);
            var sourceVideos = provider.FetchVideos();

            var videos = new List<Video>();

            foreach (var sourceVideo in sourceVideos)
            {
                if (await VideoAlreadyImported(sourceVideo))
                {
                    continue;
                }
                
                videos.Add(sourceVideo);
            }

            using var dao = GetDao();
            await dao.InsertRange(videos);
            await dao.Commit();
        }

        public async Task<IEnumerable<MediaSource>> GetAllSources()
        {
            using var dao = GetDao();
            return await dao.Query<MediaSource>().ToListAsync();
        }

        private async Task<bool> VideoAlreadyImported(Video video)
        {
            using var dao = GetDao();
            return await dao.Contains<Video>(v => v.MediaSource == video.MediaSource
                                                && v.MediaSourceUri == video.MediaSourceUri);
        }
    }
}