using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

using AudioPlayer.Event;
using AudioPlayer.ViewModel;
using AudioPlayer.ViewModel.LibraryViewModel;

namespace AudioPlayer.Model
{
    [Serializable]
    public class Library : ModelBase
    {
        public event SimpleEventHandler<string, LogMessageType, LogMessageSeverity> LogEvent;

        /// <summary>
        /// List of all library entries (non-grouped / sorted)
        /// </summary>
        public ObservableCollection<LibraryEntry> AllTitles { get; set; }

        /// <summary>
        /// Titles that have adequate information to be used by AudioPlayer, to play and organize,
        /// library entries.
        /// </summary>
        public ObservableCollection<LibraryEntry> ValidTitles { get; set; }

        /// <summary>
        /// Valid titles will add entries to this list - one per artist
        /// </summary>
        public ObservableCollection<ArtistViewModel> ValidArtists { get; set; }

        public Library()
        {
            this.AllTitles = new ObservableCollection<LibraryEntry>();
            this.ValidTitles = new ObservableCollection<LibraryEntry>();
            this.ValidArtists = new ObservableCollection<ArtistViewModel>();
        }

        /// <summary>
        /// Combined add method to update all library collections
        /// </summary>
        public void Add(LibraryEntry entry)
        {
            // All Titles
            this.AllTitles.Add(entry);

            var valid = ValidateEntry(entry);

            // Valid Titles (At least one artist, valid track data, etc..)
            if (valid)
                this.ValidTitles.Add(entry);

            // Valid Artists
            if (valid)
            {
                var artistEntry = this.ValidArtists.FirstOrDefault(x => x.Artist == entry.AlbumArtists.First().Name);

                // Existing
                if (artistEntry != null)
                {
                    // Artist -> Album(s)
                    if (!artistEntry.Albums.Any(x => x.Album == entry.Album))
                    {
                        var albumEntry = new AlbumViewModel()
                        {
                            Album = entry.Album,
                            Year = entry.Year,
                            Duration = entry.Duration
                        };

                        // IImage (Avalonia) appears to use the old System.Drawing Bitmap interface
                        //
                        albumEntry.CoverImageSource = entry.AlbumArt.FirstOrDefault();

                        // Album -> Track(s)
                        albumEntry.Tracks.Add(new TitleViewModel()
                        {
                            FileName = entry.FileName,
                            Name = entry.Title,
                            Track = entry.Track,
                            Duration = entry.Duration,
                            NowPlaying = false
                        });

                        artistEntry.Albums.Add(albumEntry);
                    }
                    else
                    {
                        // Album -> Track(s)
                        var albumEntry = artistEntry.Albums.First(x => x.Album == entry.Album);
                        albumEntry.Duration.Add(entry.Duration);
                        albumEntry.Tracks.Add(new TitleViewModel()
                        {
                            FileName = entry.FileName,
                            Name = entry.Title,
                            Track= entry.Track,
                            Duration = entry.Duration,
                            NowPlaying = false
                        });
                    }
                }

                // New
                else
                {
                    var albumEntry = new AlbumViewModel()
                    {
                        Album = entry.Album,
                        Year = entry.Year
                    };
                    artistEntry = new ArtistViewModel()
                    {
                        Artist = entry.AlbumArtists.First().Name                       
                    };

                    // IImage (Avalonia) appears to use the old System.Drawing Bitmap interface
                    //
                    albumEntry.CoverImageSource = entry.AlbumArt.FirstOrDefault();
                    artistEntry.LatestAlbumCoverSource = entry.AlbumArt.FirstOrDefault();

                    albumEntry.Tracks.Add(new TitleViewModel()
                    {
                        FileName = entry.FileName,
                        Name = entry.Title,
                        Track = entry.Track,
                        Duration = entry.Duration,
                        NowPlaying = false
                    });
                    artistEntry.Albums.Add(albumEntry);

                    this.ValidArtists.Add(artistEntry);
                }
            }

            // Log Events (TODO: Remove these, or change this library into an IEnumerable<LibraryEntry>)
            entry.LogEvent += OnEntryLog;
        }

        private bool ValidateEntry(LibraryEntry entry)
        {
            return entry != null &&
                   !string.IsNullOrEmpty(entry.FileName) &&
                   File.Exists(entry.FileName) &&
                   !string.IsNullOrEmpty(entry.Album) &&
                   entry.AlbumArtists.Any() &&
                   entry.AlbumArtists.All(x => !string.IsNullOrEmpty(x.Name)) &&
                   !string.IsNullOrEmpty(entry.AlbumArtists.First().Name);
        }

        private void OnEntryLog(string item1, LogMessageSeverity item2)
        {
            if (this.LogEvent != null)
                this.LogEvent(item1, LogMessageType.MusicBrainz, item2);
        }
    }
}
