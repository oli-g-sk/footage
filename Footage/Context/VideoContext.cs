namespace Footage.Context
{
    using System;
    using Footage.Model;
    using Microsoft.EntityFrameworkCore;

    public class VideoContext : DbContext
    {
        public DbSet<Video> Videos { get; }
        public DbSet<Bookmark> Bookmarks { get; }
        
        public string DbPath { get; }

        public VideoContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "footage.db");
        }
        
        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}