using AudioPlayer.Model;
using AudioPlayer.Model.Vendor;
using AudioPlayer.ViewModel;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace AudioPlayer;

public partial class MusicBrainzLookupView : UserControl
{
    public MusicBrainzLookupView()
    {
        InitializeComponent();

        this.DataContextChanged += MusicBrainzLookupView_DataContextChanged;
    }

    private void MusicBrainzLookupView_DataContextChanged(object sender, System.EventArgs e)
    {
        var record = this.DataContext as LibraryEntry;

        if (record != null)
        {
            this.HeaderBorder.Background = record.MusicBrainzRecordValid ? MainViewModel.ValidMusicBrainzBackground : MainViewModel.InvalidMusicBrainzBackground;
        }
        else
        {
            this.HeaderBorder.Background = MainViewModel.DefaultMusicBrainzBackground;
        }
    }

    private void OnMusicBrainzResultsChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0)
        {
            var entry = this.DataContext as LibraryEntry;
            var record = e.AddedItems[0] as MusicBrainzRecord;

            // Set selected record in the primary LibraryEntry
            entry.MusicBrainzRecord = record;
        }
    }
}