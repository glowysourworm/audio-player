using AudioPlayer.Model;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AudioPlayer.View
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            var libraryManager = LibraryManager.Create();

            this.Closing += (sender, e) =>
            {
                libraryManager.Save();
            };

            this.DataContext = libraryManager;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
