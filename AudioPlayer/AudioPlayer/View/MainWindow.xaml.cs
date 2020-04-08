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
            this.DataContext = new LibraryManager();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
