using System;
using System.Collections.ObjectModel;

using AudioPlayer.Component;
using AudioPlayer.Event;
using AudioPlayer.Extension;
using AudioPlayer.ViewModel;

namespace AudioPlayer.Model
{
    [Serializable]
    public class Library : ModelBase
    {
        SortedObservableCollection<string, LibraryEntry> _selectedStatistic;

        /// <summary>
        /// Set of problems associated with getting all tag data properly read, and installed, for
        /// each of the library entries
        /// </summary>
        public ObservableCollection<LibraryStatistic> Problems { get; set; }

        public ObservableCollection<LibraryEntry> AllTitles { get; set; }

        public event SimpleEventHandler<string, LogMessageType, LogMessageSeverity> LogEvent;

        public Library()
        {
            Initialize();
        }

        /// <summary>
        /// Combined add method to update all library collections
        /// </summary>
        public void Add(LibraryEntry entry)
        {
            // All Titles
            this.AllTitles.Add(entry);

            // Statistics 
            foreach (var statistic in this.Problems)
                statistic.FilteredAdd(entry);

            // Log Events (TODO: Remove these, or change this library into an IEnumerable<LibraryEntry>)
            entry.LogEvent += OnEntryLog;
        }

        private void Initialize()
        {
            this.AllTitles = new ObservableCollection<LibraryEntry>();
            this.Problems = new ObservableCollection<LibraryStatistic>();

            // Statistics
            /*
            this.Problems.Add(new LibraryStatistic("Files Scanned", x => x.FileName, x => true));

            this.Problems.Add(new LibraryStatistic("Album Unknown", x => x.FileName, x => x.IsUnknown(z => z.Album)));
            this.Problems.Add(new LibraryStatistic("Album Artist Unknown", x => x.FileName, x => x.IsUnknown(z => z.AlbumArtists)));
            this.Problems.Add(new LibraryStatistic("Genre Unknown", x => x.FileName, x => x.IsUnknown(z => z.Genres)));
            this.Problems.Add(new LibraryStatistic("Title Unknown", x => x.FileName, x => x.IsUnknown(z => z.Title)));
            this.Problems.Add(new LibraryStatistic("Year Unknown", x => x.FileName, x => x.IsUnknown(z => z.Year)));
            this.Problems.Add(new LibraryStatistic("Track Unknown", x => x.FileName, x => x.IsUnknown(z => z.Track)));
            this.Problems.Add(new LibraryStatistic("Disc Unknown", x => x.FileName, x => x.IsUnknown(z => z.Disc)));
            this.Problems.Add(new LibraryStatistic("Disc Count Unknown", x => x.FileName, x => x.IsUnknown(z => z.DiscCount)));
            //this.Problems.Add(new LibraryStatistic("Artwork Found", x => x.FileName, x => _libraryFile.ContainsArtworkFor(x)));
            */
        }

        private void OnEntryLog(string item1, LogMessageSeverity item2)
        {
            if (this.LogEvent != null)
                this.LogEvent(item1, LogMessageType.MusicBrainz, item2);
        }
    }
}
