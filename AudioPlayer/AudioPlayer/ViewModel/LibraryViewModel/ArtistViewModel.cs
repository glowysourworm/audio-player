using System.Collections.ObjectModel;

using AudioPlayer.Model;

using Avalonia.Controls;
using Avalonia.Media;

namespace AudioPlayer.ViewModel.LibraryViewModel
{
    /// <summary>
    /// View model for viewing LibraryEntry data by artist
    /// </summary>
    public class ArtistViewModel : ModelBase
    {
        string _artist;
        ObservableCollection<AlbumViewModel> _albums;
        IImage _latestAlbumCoverSource;

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
        public IImage LatestAlbumCoverSource
        {
            get { return _latestAlbumCoverSource; }
            set { this.SetProperty(ref _latestAlbumCoverSource, value); }
        }

        public ArtistViewModel()
        {
            this.Artist = string.Empty;
            this.Albums = new ObservableCollection<AlbumViewModel>();
            this.LatestAlbumCoverSource = null;
        }
    }
}
