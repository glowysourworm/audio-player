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

            // DEBUG MP3 DATA
            var library = new Library();

            library.OpenDirectoryEvent += async () =>
            {
                var dialog = new OpenFolderDialog();

                var result = await dialog.ShowAsync(this);

                if (!string.IsNullOrEmpty(result))
                {
                    var directories = new string[] { result };

                    library.Open(directories);
                }
            };

            this.DataContext = library;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
