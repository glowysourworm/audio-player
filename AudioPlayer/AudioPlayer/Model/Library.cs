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
        #endregion

        public Library()
        {
            this.ScanStatus = GetScanStatus(0, 0);

            this.Directories = new SortedObservableCollection<string, string>(x => x, false);

            this.AllTitles = new SortedObservableCollection<string, LibraryEntry>(x => x.Title, true);
            this.FilteredTitles = new SortedObservableCollection<string, LibraryEntry>(x => x.Title, true);

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
                    }

                    // Update Scan Status
                    this.ScanStatus = GetScanStatus(countCompleted, countTotal);

                }, DispatcherPriority.DataBind);
            };

            scanner.Scan(directories, "*.mp3");
        }

        private string GetScanStatus(int completed, int total)
        {
            return string.Format("({0} of {1}) files scanned...", completed, total);
        }
    }
}
