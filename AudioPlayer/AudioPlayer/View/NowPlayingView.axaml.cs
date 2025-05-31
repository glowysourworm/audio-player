using System.Linq;

using AudioPlayer.Model;
using AudioPlayer.ViewModel.LibraryViewModel;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AudioPlayer;

public partial class NowPlayingView : UserControl
{
    public NowPlayingView()
    {
        InitializeComponent();
    }

    private void OnArtistSelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        var library = this.DataContext as Library;

        if (e.AddedItems.Count > 0 && library != null)
        {
            var artistViewModel = e.AddedItems[0] as ArtistViewModel;

            // View Artist Detail
            this.ArtistDetailLB.ItemsSource = artistViewModel.Albums;
        }
    }

    private void OnArtistDetailDoubleClick(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        var libraryEntry = (e.Source as Control).DataContext as LibraryEntry;
        var library = this.DataContext as Library;

        if (libraryEntry != null)
        {
            // Load album for this title into playlist / start playing this title
            var artistAlbumTracks = library.ValidTitles.Where(x => x.Album == libraryEntry.Album).ToList();

            this.PlaylistLB.ItemsSource = artistAlbumTracks;
        }
    }
}