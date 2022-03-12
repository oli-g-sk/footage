namespace Footage.Context
{
    using System;
    using Footage.Model;
    using Microsoft.EntityFrameworkCore;

    public class VideoContext : DbContext
    {
        private readonly string dbPath;
        
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        // NOTE: EFCore needs the setter, otherwise the properties remain null 
        
        public DbSet<Video>? Videos { get; set; }
        public DbSet<Bookmark>? Bookmarks { get; set; }
        public DbSet<MediaSource>? MediaSources { get; set; }
        public DbSet<Project>? Projects { get; set; }
        
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public VideoContext()
        {
            string? path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            dbPath = System.IO.Path.Join(path, "footage.db");
        }
        
        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={dbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeBookmark>();
            modelBuilder.Entity<RangeBookmark>();
            modelBuilder.Entity<LocalMediaSource>();
            
            base.OnModelCreating(modelBuilder);
        }
    }
}