using AudioPlayer.Component;
using AudioPlayer.Event;
using AudioPlayer.Extension;

using Avalonia.Threading;

using ReactiveUI;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

namespace AudioPlayer.Model
{
    [Serializable]
    public class Library : ModelBase
    {
        FileScanner _scanner;

        #region (public) Properties
        string _scanStatus;

        string _filteredGenre;
        string _filteredArtist;
        string _filteredAlbum;

        SortedObservableCollection<string, LibraryEntry> _selectedStatistic;

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

        public IReactiveCommand OpenDirectoryCommand { get; set; }

        public event SimpleEventHandler OpenDirectoryEvent;
        #endregion

        #region (public) Library Collections
        public SortedObservableCollection<string, string> Directories { get; set; }

        public SortedObservableCollection<string, LibraryEntry> AllTitles { get; set; }
        public SortedObservableCollection<string, LibraryEntry> FilteredTitles { get; set; }

        public ObservableCollection<LibraryStatistic> Statistics { get; set; }

        public SortedObservableCollection<string, LibraryEntry> SelectedStatistic
        { 
            get { return _selectedStatistic; } 
            set { Update(ref _selectedStatistic, value); }
        }
        #endregion

        public Library()
        {
            _scanner = new FileScanner();

            this.ScanStatus = GetScanStatus(0, 0);

            this.Directories = new SortedObservableCollection<string, string>(x => x, false);

            this.AllTitles = new SortedObservableCollection<string, LibraryEntry>(x => x.Title, true);
            this.FilteredTitles = new SortedObservableCollection<string, LibraryEntry>(x => x.Title, true);
            this.Statistics = new ObservableCollection<LibraryStatistic>();

            // Statistics
            this.Statistics.Add(new LibraryStatistic("Files Scanned", x => x.FileName, x => true, false));
            this.Statistics.Add(new LibraryStatistic("Files Valid", x => x.FileName, x => x.IsValid, false));
            this.Statistics.Add(new LibraryStatistic("Complete Entries", x => x.FileName, x => x.IsComplete, false));

            this.Statistics.Add(new LibraryStatistic("Album Unknown", x => x.FileName, 
                                                                      x => x.IsUnknown(z => z.Album), 
                                                                      false));

            this.Statistics.Add(new LibraryStatistic("Album Artist Unknown", x => x.FileName,
                                                                             x => x.IsUnknown(z => z.AlbumArtists),
                                                                             false));

            this.Statistics.Add(new LibraryStatistic("Genre Unknown", x => x.FileName,
                                                                      x => x.IsUnknown(z => z.Genres),
                                                                      false));

            this.Statistics.Add(new LibraryStatistic("Lyrics Unknown", x => x.FileName,
                                                                       x => x.IsUnknown(z => z.Lyrics),
                                                                       false));

            this.Statistics.Add(new LibraryStatistic("Title Unknown", x => x.FileName,
                                                                      x => x.IsUnknown(z => z.Title),
                                                                      false));

            this.Statistics.Add(new LibraryStatistic("Year Unknown", x => x.FileName,
                                                                     x => x.IsUnknown(z => z.Year),
                                                                     false));

            this.Statistics.Add(new LibraryStatistic("Track Unknown", x => x.FileName,
                                                                      x => x.IsUnknown(z => z.Track),
                                                                      false));

            this.Statistics.Add(new LibraryStatistic("Track Count Unknown", x => x.FileName,
                                                                            x => x.IsUnknown(z => z.TrackCount),
                                                                            false));

            this.Statistics.Add(new LibraryStatistic("Disc Unknown", x => x.FileName,
                                                                     x => x.IsUnknown(z => z.Disc),
                                                                     false));

            this.Statistics.Add(new LibraryStatistic("Disc Count Unknown", x => x.FileName,
                                                                            x => x.IsUnknown(z => z.DiscCount),
                                                                            false));

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

            // Reset statistics
            foreach (var statistic in this.Statistics)
                statistic.Clear();

            var counter = 0;

            _scanner.FileScannedEvent += (entry, count) =>
            {
                if (!entry.IsValid)
                    return;

                // Atomic increment
                Interlocked.Increment(ref counter);

                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    // All Titles
                    this.AllTitles.Add(entry);

                    // Statistics 
                    foreach (var statistic in this.Statistics)
                        statistic.FilteredAdd(entry);

                    this.ScanStatus = GetScanStatus(counter, count);

                }, DispatcherPriority.Render);
            };

            _scanner.Scan(directories, "*.mp3");
        }

        private string GetScanStatus(int completed, int total)
        {
            return string.Format("({0} of {1}) files scanned...", completed, total);
        }
    }
}
