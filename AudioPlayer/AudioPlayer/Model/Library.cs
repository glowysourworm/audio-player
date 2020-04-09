using AudioPlayer.Event;
using AudioPlayer.Extension;
using AudioPlayer.Model.Database;
using ReactiveUI;

using System;
using System.Collections.ObjectModel;

namespace AudioPlayer.Model
{
    [Serializable]
    public class Library : ModelBase
    {
        readonly LibraryFile _libraryFile;

        SortedObservableCollection<string, LibraryEntry> _selectedStatistic;

        public ObservableCollection<LibraryStatistic> Statistics { get; set; }
        public SortedObservableCollection<string, LibraryEntry> AllTitles { get; set; }
        public SortedObservableCollection<string, LibraryEntry> SelectedStatistic
        {
            get { return _selectedStatistic; }
            set { Update(ref _selectedStatistic, value); }
        }

        public LibraryFile Database { get { return _libraryFile; } }

        public Library(LibraryFile libraryFile)
        {
            _libraryFile = libraryFile;

            Initialize();

            foreach (var entry in libraryFile.Entries)
                Add(entry);
        }

        /// <summary>
        /// Combined add method to update all library collections
        /// </summary>
        public void Add(LibraryEntry entry)
        {
            // All Titles
            this.AllTitles.Add(entry, x => x.FileName);

            // Statistics 
            foreach (var statistic in this.Statistics)
                statistic.FilteredAdd(entry);
        }

        private void Initialize()
        {
            this.AllTitles = new SortedObservableCollection<string, LibraryEntry>();
            this.Statistics = new ObservableCollection<LibraryStatistic>();

            // Statistics
            this.Statistics.Add(new LibraryStatistic("Files Scanned", x => x.FileName, x => true));
            this.Statistics.Add(new LibraryStatistic("Files Valid", x => x.FileName, x => x.IsValid));
            this.Statistics.Add(new LibraryStatistic("Complete Entries", x => x.FileName, x => x.IsComplete));

            this.Statistics.Add(new LibraryStatistic("Album Unknown", x => x.FileName, x => x.IsUnknown(z => z.Album)));
            this.Statistics.Add(new LibraryStatistic("Album Artist Unknown", x => x.FileName, x => x.IsUnknown(z => z.AlbumArtists)));
            this.Statistics.Add(new LibraryStatistic("Genre Unknown", x => x.FileName, x => x.IsUnknown(z => z.Genres)));
            this.Statistics.Add(new LibraryStatistic("Lyrics Unknown", x => x.FileName, x => x.IsUnknown(z => z.Lyrics)));
            this.Statistics.Add(new LibraryStatistic("Title Unknown", x => x.FileName, x => x.IsUnknown(z => z.Title)));
            this.Statistics.Add(new LibraryStatistic("Year Unknown", x => x.FileName, x => x.IsUnknown(z => z.Year)));
            this.Statistics.Add(new LibraryStatistic("Track Unknown", x => x.FileName, x => x.IsUnknown(z => z.Track)));
            this.Statistics.Add(new LibraryStatistic("Track Count Unknown", x => x.FileName, x => x.IsUnknown(z => z.TrackCount)));
            this.Statistics.Add(new LibraryStatistic("Disc Unknown", x => x.FileName, x => x.IsUnknown(z => z.Disc)));
            this.Statistics.Add(new LibraryStatistic("Disc Count Unknown", x => x.FileName, x => x.IsUnknown(z => z.DiscCount)));
            this.Statistics.Add(new LibraryStatistic("Artwork Found", x => x.FileName, x => x.ArtworkResolved != null));
        }
    }
}
