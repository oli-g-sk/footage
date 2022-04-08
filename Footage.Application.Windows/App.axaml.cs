using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Footage.Application.UI.Views;

namespace Footage.Application.Windows
{
    using Application = Avalonia.Application;

    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            UI.Core.Initialize();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = new MainWindow();
                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}