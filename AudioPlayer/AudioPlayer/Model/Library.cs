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
        SortedObservableCollection<string, LibraryEntry> _selectedStatistic;

        public ObservableCollection<LibraryStatistic> Statistics { get; set; }
        public SortedObservableCollection<string, string> Directories { get; set; }
        public SortedObservableCollection<string, LibraryEntry> AllTitles { get; set; }
        public SortedObservableCollection<string, LibraryEntry> SelectedStatistic
        {
            get { return _selectedStatistic; }
            set { Update(ref _selectedStatistic, value); }
        }

        public Library()
        {
            Initialize();
        }

        public Library(LibraryFile libraryFile)
        {
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
            this.AllTitles.Add(entry);

            // Statistics 
            foreach (var statistic in this.Statistics)
                statistic.FilteredAdd(entry);
        }

        private void Initialize()
        {
            this.Directories = new SortedObservableCollection<string, string>(x => x, false);

            this.AllTitles = new SortedObservableCollection<string, LibraryEntry>(x => x.Title, true);
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

            this.Statistics.Add(new LibraryStatistic("Artwork Found", x => x.FileName,
                                                                      x => x.ArtworkResolved != null,
                                                                      false));
        }
    }
}
