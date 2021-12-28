namespace Footage.ViewModel.Base
{
    using System;
    using Footage.Messages;
    using Footage.ViewModel.Entity;
    using JetBrains.Annotations;
    using LibVLCSharp.Shared;

    public sealed class SelectedVideoViewModel : SectionViewModel, IDisposable
    {
        public event EventHandler? BeforeVideoChanged;

        public event EventHandler? AfterVideoChanged;

        public MediaPlayer Player { get; }

        private VideoViewModel? selectedVideo;

        [CanBeNull]
        public VideoViewModel? SelectedVideo
        {
            get => selectedVideo;
            set
            {
                BeforeVideoChanged?.Invoke(this, EventArgs.Empty);
                Set(ref selectedVideo, value);
                AfterVideoChanged?.Invoke(this, EventArgs.Empty);
            } 
        }

        public SelectedVideoViewModel()
        {
            Player = new MediaPlayer(Locator.LibVlc);
            MessengerInstance.Register<SelectionChangedMessage<VideoViewModel>>(this, m => SelectedVideo = m.SelectedItem);
        }

        public void Dispose()
        {
            Player?.Dispose();
        }
    }
}