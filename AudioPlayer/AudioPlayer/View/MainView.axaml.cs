using AudioPlayer.ViewModel;

using Avalonia.Controls;

namespace AudioPlayer;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        this.DataContext = new MainViewModel();

        // Bindings weren't working (tried ViewLocator, CompiledBindings)
        this.ConfigurationView.DataContext = (this.DataContext as MainViewModel).Configuration;
        this.ManagerView.DataContext = this.DataContext;
        this.OutputLB.ItemsSource = (this.DataContext as MainViewModel).OutputMessages;
        
    }
}