using System.Collections.Generic;
using System.Linq;

using AudioPlayer.Controller;
using AudioPlayer.Controller.Interface;
using AudioPlayer.Model;
using AudioPlayer.ViewModel.LibraryViewModel;

using Avalonia.Controls;

namespace AudioPlayer;

public partial class NowPlayingView : UserControl
{
    // TODO: Dependency Injection
    readonly IAudioController _audioController;

    public NowPlayingView()
    {
        InitializeComponent();

        _audioController = new AudioController();
    }

    private void OnArtistSelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        var library = this.DataContext as Library;

        if (e.AddedItems.Count > 0 && library != null)
        {
            // View Artist Detail
            this.ArtistDetailLB.ItemsSource = (e.AddedItems[0] as ArtistViewModel).Albums;
        }
    }

    private void OnArtistDetailDoubleClick(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        var selectedTrack = (e.Source as Control).DataContext as TitleViewModel;

        if (selectedTrack != null)
        {
            var albums = this.ArtistDetailLB.ItemsSource as IEnumerable<AlbumViewModel>;
            var selectedAlbum = albums.First(album => album.Tracks.Contains(selectedTrack));

            foreach (var track in albums.SelectMany(x => x.Tracks))
            {
                track.NowPlaying = (track == selectedTrack);
            }

            // Play Selected Track
            _audioController.Play(selectedTrack.FileName);
        }
    }
}