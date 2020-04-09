using AudioPlayer.Component;
using AudioPlayer.Event;
using AudioPlayer.Extension;
using AudioPlayer.Model.Database;
using AudioPlayer.Model.Interface;
using ReactiveUI;

using System;
using System.Collections.ObjectModel;

namespace AudioPlayer.Model
{
    [Serializable]
    public class Library : ModelBase<Library>
    {
        readonly LibraryFile _libraryFile;

        SortedObservableCollection<string, ILibraryEntry> _selectedStatistic;

        public ObservableCollection<LibraryStatistic> Statistics { get; set; }
        public SortedObservableCollection<string, ILibraryEntry> AllTitles { get; set; }
        public SortedObservableCollection<string, ILibraryEntry> SelectedStatistic
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
        public void Add(ILibraryEntry entry)
        {
            // All Titles
            this.AllTitles.Add(entry, x => x.FileName);

            // Statistics 
            foreach (var statistic in this.Statistics)
                statistic.FilteredAdd(entry);
        }

        private void Initialize()
        {
            this.AllTitles = new SortedObservableCollection<string, ILibraryEntry>();
            this.Statistics = new ObservableCollection<LibraryStatistic>();

            // Statistics
            this.Statistics.Add(new LibraryStatistic("Files Scanned", x => x.FileName, x => true));

            this.Statistics.Add(new LibraryStatistic("Album Unknown", x => x.FileName, x => x.IsUnknown(z => z.Album)));
            this.Statistics.Add(new LibraryStatistic("Album Artist Unknown", x => x.FileName, x => x.IsUnknown(z => z.AlbumArtists)));
            this.Statistics.Add(new LibraryStatistic("Genre Unknown", x => x.FileName, x => x.IsUnknown(z => z.Genres)));
            this.Statistics.Add(new LibraryStatistic("Title Unknown", x => x.FileName, x => x.IsUnknown(z => z.Title)));
            this.Statistics.Add(new LibraryStatistic("Year Unknown", x => x.FileName, x => x.IsUnknown(z => z.Year)));
            this.Statistics.Add(new LibraryStatistic("Track Unknown", x => x.FileName, x => x.IsUnknown(z => z.Track)));
            this.Statistics.Add(new LibraryStatistic("Disc Unknown", x => x.FileName, x => x.IsUnknown(z => z.Disc)));
            this.Statistics.Add(new LibraryStatistic("Disc Count Unknown", x => x.FileName, x => x.IsUnknown(z => z.DiscCount)));
            this.Statistics.Add(new LibraryStatistic("Artwork Found", x => x.FileName, x => _libraryFile.ContainsArtworkFor(x)));
        }
    }
}
