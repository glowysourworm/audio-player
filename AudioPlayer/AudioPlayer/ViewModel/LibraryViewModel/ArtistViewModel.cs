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
        string _fileNameRef;

        string _artist;
        ObservableCollection<AlbumViewModel> _albums;

        /// <summary>
        /// Reference to the Mp3 file. The album art is too large to pre-load. So, loading will have
        /// to be accomplished on the fly.
        /// </summary>
        public string FileNameRef
        {
            get { return _fileNameRef; }
            set { this.SetProperty(ref _fileNameRef, value); }
        }

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

        public ArtistViewModel()
        {
            this.Artist = string.Empty;
            this.Albums = new ObservableCollection<AlbumViewModel>();
            this.FileNameRef = string.Empty;
        }
    }
}
