using System.Collections.ObjectModel;

using AudioPlayer.Model;

using Avalonia.Controls;

namespace AudioPlayer.ViewModel.LibraryViewModel
{
    /// <summary>
    /// View model for viewing LibraryEntry data by artist
    /// </summary>
    public class ArtistViewModel : ModelBase
    {
        string _artist;
        ObservableCollection<AlbumViewModel> _albums;
        Image _latestAlbumCover;

        public string Artist
        {
            get { return _artist; }
            set { this.SetProperty(ref _artist, value); }
        }
        public ObservableCollection<AlbumViewModel> Albums
        {
            get { return _albums; }
            set { this.SetProperty(ref _albums, value); }
        }
        public Image LatestAlbumCover
        {
            get { return _latestAlbumCover; }
            set { this.SetProperty(ref _latestAlbumCover, value); }
        }

        public ArtistViewModel()
        {
            this.Artist = string.Empty;
            this.Albums = new ObservableCollection<AlbumViewModel>();
            this.LatestAlbumCover = new Image();
        }
    }
}
