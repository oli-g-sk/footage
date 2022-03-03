namespace Footage.UI.Views.MainWindowContent
{
    using System;
    using System.Diagnostics;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Controls.Mixins;
    using Avalonia.Markup.Xaml;
    using Footage.ViewModel.Section;

    public class VideoBrowserView : UserControl
    {
        private readonly CompositeDisposable disposables = new();

        private CompositeDisposable? scrollViewerDisposables;
        
        private double verticalHeightMax;

        private VideoBrowserViewModel ViewModel => DataContext as VideoBrowserViewModel;
        
        public VideoBrowserView()
        {
            InitializeComponent();
            
            var listBox = this.Find<ListBox>("ListBox");
            listBox.GetObservable(ListBox.ScrollProperty)
                .OfType<ScrollViewer>()
                .Take(1)
                .Subscribe(sv =>
                {
                    scrollViewerDisposables?.Dispose();
                    scrollViewerDisposables = new CompositeDisposable();
                    
                    sv.GetObservable(ScrollViewer.VerticalScrollBarMaximumProperty)
                        .Subscribe(newMax => verticalHeightMax = newMax)
                        .DisposeWith(scrollViewerDisposables);

                    sv.GetObservable(ScrollViewer.OffsetProperty)
                        .Subscribe(offset =>
                        {
                            if (offset.Y <= Double.Epsilon)
                            {
                                Debug.WriteLine("At Top");
                            }

                            var delta = Math.Abs(verticalHeightMax - offset.Y);
                            if (delta <= Double.Epsilon)
                            {
                                Debug.WriteLine("At Bottom");
                                ViewModel.FetchMoreCommand.Execute(null);
                            }
                        }).DisposeWith(disposables);
                });
        }
        
        /* TODO dispose
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _scrollViewerDisposables.Dispose();
            _disposables.Dispose();
        }
        */

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}