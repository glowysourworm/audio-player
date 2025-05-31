using AudioPlayer.Model;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AudioPlayer;

public partial class ManagerView : UserControl
{
    public ManagerView()
    {
        InitializeComponent();
    }

    private void ListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0)
        {
            var item = e.AddedItems[0] as LibraryEntry;

            this.LocalEntryItemView.DataContext = item;
            this.MusicBrainzItemView.DataContext = item;
        }
    }
}