﻿namespace Footage.Engine
{
    using System;
    using System.Threading.Tasks;

    public interface IMediaPlayerService
    {
        event EventHandler PositionChanged;

        public float Position { get; set; }

        public long Duration { get; }

        bool IsPlaying { get; }

        bool IsMediaLoaded { get; }

        Task Play();

        Task Pause();

        Task Stop();

        Task LoadMedia(string uri);
        
        Task UnloadMedia();

        Task<long> GetVideoDuration(string videoUri);
    }
}