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
        private double verticalHeightMax;

        private VideoBrowserViewModel ViewModel => DataContext as VideoBrowserViewModel;
        
        public VideoBrowserView()
        {
            InitializeComponent();
            
            var listBox = this.Find<ListBox>("ListBox");
            listBox.GetObservable(ListBox.ScrollProperty)
                .OfType<ScrollViewer>()
                .Take(1)
                .Subscribe(CreateScrollViewerSubscriptions);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void CreateScrollViewerSubscriptions(ScrollViewer scrollViewer)
        {
            scrollViewer.GetObservable(ScrollViewer.VerticalScrollBarMaximumProperty)
                .Subscribe(newMax => verticalHeightMax = newMax);

            scrollViewer.GetObservable(ScrollViewer.OffsetProperty)
                .Subscribe(CalculateScrollDelta);
        }

        private void CalculateScrollDelta(Vector offset)
        {
            if (offset.Y <= double.Epsilon)
            {
                Debug.WriteLine("At Top");
            }

            double delta = Math.Abs(verticalHeightMax - offset.Y);
            
            if (delta <= double.Epsilon)
            {
                Debug.WriteLine("At Bottom");
                ViewModel.FetchMoreCommand.Execute(null);
            }
        }
    }
}