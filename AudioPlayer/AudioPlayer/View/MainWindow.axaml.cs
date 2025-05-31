using AudioPlayer.ViewModel;

using Avalonia.Controls;

namespace AudioPlayer;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        this.DataContext = new MainViewModel();
    }
}