﻿namespace Footage.Service
{
    using System.Threading.Tasks;
    using LibVLCSharp.Shared;

    public interface IMediaPlayerService
    {
        static IMediaPlayerService Instance { get; }
        
        public MediaPlayer Player { get; }

        Task LoadMedia(string uri);
        
        Task UnloadMedia();

        Task<long> GetVideoDuration(string videoUri);
    }
}