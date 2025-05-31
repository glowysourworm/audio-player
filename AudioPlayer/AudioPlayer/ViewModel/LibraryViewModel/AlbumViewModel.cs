using System;
using System.Collections.ObjectModel;

using AudioPlayer.Extension;
using AudioPlayer.Model;
using AudioPlayer.Model.Comparer;

using Avalonia.Controls;

namespace AudioPlayer.ViewModel.LibraryViewModel
{
    /// <summary>
    /// Component used to view album details in the application (for valid library entries)
    /// </summary>
    public class AlbumViewModel : ModelBase
    {
        string _album;
        uint _year;
        TimeSpan _duration;
        Image _coverImage;
        SortedObservableCollection<LibraryEntry> _tracks;

        public string Album
        {
            get { return _album; }
            set { this.SetProperty(ref _album, value); }
        }
        public uint Year
        {
            get { return _year; }
            set { this.SetProperty(ref _year, value); }
        }
        public Image CoverImage
        {
            get { return _coverImage; }
            set { this.SetProperty(ref _coverImage, value); }
        }
        public TimeSpan Duration
        {
            get { return _duration; }
            set { this.SetProperty(ref _duration, value); }
        }
        public SortedObservableCollection<LibraryEntry> Tracks
        {
            get { return _tracks; }
            set { this.SetProperty(ref _tracks, value); }
        }

        public AlbumViewModel()
        {
            this.Album = string.Empty;
            this.CoverImage = null;
            this.Duration = new TimeSpan();
            this.Year = 0;
            this.Tracks = new SortedObservableCollection<LibraryEntry>(new TrackNumberComparer());
        }
    }
}
