using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AudioPlayer.View;
using Avalonia.Controls;

namespace AudioPlayer
{
    public class App : Application
    {
        /// <summary>
        /// Convenience method for desktop application (ONLY)
        /// </summary>
        public static Window MainWindow { get; private set; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();

                App.MainWindow = desktop.MainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
