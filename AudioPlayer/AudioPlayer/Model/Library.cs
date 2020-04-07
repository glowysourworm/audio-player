using AudioPlayer.Component;
using AudioPlayer.Event;
using AudioPlayer.Extension;

using Avalonia.Threading;

using ReactiveUI;

using System;
using System.Linq;
using System.Reactive.Linq;

namespace AudioPlayer.Model
{
    [Serializable]
    public class Library : ModelBase
    {
        #region (public) Properties
        string _scanStatus;

        string _filteredGenre;
        string _filteredArtist;
        string _filteredAlbum;

        LibraryStatistics _statistics;

        public string ScanStatus
        {
            get { return _scanStatus; }
            set { Update(ref _scanStatus, value); }
        }
        public string FilteredGenre
        {
            get { return _filteredGenre; }
            set { Update(ref _filteredGenre, value); }
        }
        public string FilteredArtist
        {
            get { return _filteredArtist; }
            set { Update(ref _filteredArtist, value); }
        }
        public string FilteredAlbum
        {
            get { return _filteredAlbum; }
            set { Update(ref _filteredAlbum, value); }
        }

        public LibraryStatistics Statistics
        {
            get { return _statistics; }
            set { Update(ref _statistics, value); }
        }

        public IReactiveCommand OpenDirectoryCommand { get; set; }

        public event SimpleEventHandler OpenDirectoryEvent;
        #endregion

        #region (public) Library Collections
        public SortedObservableCollection<string, string> Directories { get; set; }

        public SortedObservableCollection<string, LibraryEntry> AllTitles { get; set; }
        public SortedObservableCollection<string, LibraryEntry> FilteredTitles { get; set; }
        #endregion

        public Library()
        {
            this.ScanStatus = GetScanStatus(0, 0);

            this.Directories = new SortedObservableCollection<string, string>(x => x, false);

            this.AllTitles = new SortedObservableCollection<string, LibraryEntry>(x => x.Title, true);
            this.FilteredTitles = new SortedObservableCollection<string, LibraryEntry>(x => x.Title, true);
            this.Statistics = new LibraryStatistics();

            this.OpenDirectoryCommand = ReactiveCommand.Create(() =>
            {
                if (this.OpenDirectoryEvent != null)
                    this.OpenDirectoryEvent();
            });
        }

        /// <summary>
        /// Scans specified directories to add to library
        /// </summary>
        public void Open(string[] directories)
        {
            // Create new library directories
            this.Directories.ReCreate(directories.Distinct());

            // TOOD: Block new scan until existing one is completed

            this.AllTitles.Clear();
            this.FilteredTitles.Clear();
            this.ScanStatus = GetScanStatus(0, 0);
            this.Statistics.Clear();

            var scanner = new FileScanner();

            scanner.ScanUpdateEvent += (entriesCompleted, countCompleted, countTotal) =>
            {
                // Invoke UI thread -> Update bound collections
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    // Completed entries - detached from scanner
                    foreach (var entry in entriesCompleted.Where(x => x.IsValid).Actualize())
                    {
                        // TODO: HANDLE DUPLICATE ENTRIES
                        if (this.AllTitles.Contains(entry))
                            continue;

                        // All Titles
                        this.AllTitles.Add(entry);

                        // Filtered By Genre
                        if (!string.IsNullOrEmpty(_filteredGenre) &&
                            entry.Genres.Contains(_filteredGenre))
                            this.FilteredTitles.Add(entry);

                        // Filtered By Artist
                        else if (!string.IsNullOrEmpty(_filteredGenre) &&
                            entry.Genres.Contains(_filteredGenre))
                            this.FilteredTitles.Add(entry);

                        // Filtered By Album
                        else if (!string.IsNullOrEmpty(_filteredGenre) &&
                            entry.Genres.Contains(_filteredGenre))
                            this.FilteredTitles.Add(entry);

                        // No Filter applied
                        else
                            this.FilteredTitles.Add(entry);

                        // STATISTICS

                        // File Scanned
                        this.Statistics.TotalFilesScanned++;
                        this.Statistics.TotalCompleteEntries += entry.IsComplete ? 1 : 0;
                        this.Statistics.TotalFilesEmpty += entry.IsEmpty ? 1 : 0;
                        this.Statistics.TotalFilesValid += entry.IsValid ? 1 : 0;

                        // Field Unknown
                        this.Statistics.TotalAlbumArtistUnknown += entry.IsUnknown(x => x.AlbumArtists) ? 1 : 0;
                        this.Statistics.TotalAlbumUnknown += entry.IsUnknown(x => x.Album) ? 1 : 0;
                        this.Statistics.TotalDiscCountUnknown += entry.IsUnknown(x => x.DiscCount) ? 1 : 0;
                        this.Statistics.TotalDiscUnknown += entry.IsUnknown(x => x.Disc) ? 1 : 0;
                        this.Statistics.TotalGenreUnknown += entry.IsUnknown(x => x.Genres) ? 1 : 0;
                        this.Statistics.TotalLyricsUnknown += entry.IsUnknown(x => x.Lyrics) ? 1 : 0;
                        this.Statistics.TotalTitleUnknown += entry.IsUnknown(x => x.Title) ? 1 : 0;
                        this.Statistics.TotalTrackCountUnknown += entry.IsUnknown(x => x.TrackCount) ? 1 : 0;
                        this.Statistics.TotalTrackUnknown += entry.IsUnknown(x => x.Track) ? 1 : 0;
                        this.Statistics.TotalYearUnknown += entry.IsUnknown(x => x.Year) ? 1 : 0;
                    }

                    // Update Scan Status
                    this.ScanStatus = GetScanStatus(countCompleted, countTotal);

                }, DispatcherPriority.MaxValue);
            };

            scanner.Scan(directories, "*.mp3");
        }

        private string GetScanStatus(int completed, int total)
        {
            return string.Format("({0} of {1}) files scanned...", completed, total);
        }
    }
}
