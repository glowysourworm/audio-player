using AudioPlayer.Extension;
using System;

namespace AudioPlayer.Model
{
	[Serializable]
	public class LibraryStatistics : ModelBase
	{
		// Valid = ??? + ??? + ???

		public SortedObservableCollection<string, LibraryEntry> FilesScanned { get; set; }
		public SortedObservableCollection<string, LibraryEntry> FilesValid { get; set; }
		public SortedObservableCollection<string, LibraryEntry> FilesEmpty { get; set; }
		public SortedObservableCollection<string, LibraryEntry> CompleteEntries { get; set; }
		public SortedObservableCollection<string, LibraryEntry> AlbumUnknown { get; set; }
		public SortedObservableCollection<string, LibraryEntry> AlbumArtistUnknown { get; set; }
		public SortedObservableCollection<string, LibraryEntry> GenreUnknown { get; set; }
		public SortedObservableCollection<string, LibraryEntry> LyricsUnknown { get; set; }
		public SortedObservableCollection<string, LibraryEntry> TitleUnknown { get; set; }
		public SortedObservableCollection<string, LibraryEntry> YearUnknown { get; set; }
		public SortedObservableCollection<string, LibraryEntry> TrackUnknown { get; set; }
		public SortedObservableCollection<string, LibraryEntry> TrackCountUnknown { get; set; }
		public SortedObservableCollection<string, LibraryEntry> DiscUnknown { get; set; }
		public SortedObservableCollection<string, LibraryEntry> DiscCountUnknown { get; set; }

		public LibraryStatistics() 
		{
			this.FilesScanned = new SortedObservableCollection<string, LibraryEntry>(x => x.FileName, false);
			this.FilesValid = new SortedObservableCollection<string, LibraryEntry>(x => x.FileName, false);
			this.FilesEmpty = new SortedObservableCollection<string, LibraryEntry>(x => x.FileName, false);
			this.CompleteEntries = new SortedObservableCollection<string, LibraryEntry>(x => x.FileName, false);
			this.AlbumUnknown = new SortedObservableCollection<string, LibraryEntry>(x => x.FileName, false);
			this.AlbumArtistUnknown = new SortedObservableCollection<string, LibraryEntry>(x => x.FileName, false);
			this.GenreUnknown = new SortedObservableCollection<string, LibraryEntry>(x => x.FileName, false);
			this.LyricsUnknown = new SortedObservableCollection<string, LibraryEntry>(x => x.FileName, false);
			this.TitleUnknown = new SortedObservableCollection<string, LibraryEntry>(x => x.FileName, false);
			this.YearUnknown = new SortedObservableCollection<string, LibraryEntry>(x => x.FileName, false);
			this.TrackUnknown = new SortedObservableCollection<string, LibraryEntry>(x => x.FileName, false);
			this.TrackCountUnknown = new SortedObservableCollection<string, LibraryEntry>(x => x.FileName, false);
			this.DiscUnknown = new SortedObservableCollection<string, LibraryEntry>(x => x.FileName, false);
			this.DiscCountUnknown = new SortedObservableCollection<string, LibraryEntry>(x => x.FileName, false);
		}

		/// <summary>
		/// Adds entry to applicable collections
		/// </summary>
		public void Add(LibraryEntry entry)
		{
			this.FilesScanned.Add(entry);

			// IsComplete
			if (entry.IsComplete)
				this.CompleteEntries.Add(entry);

			// IsEmpty
			if (entry.IsEmpty)
				this.FilesEmpty.Add(entry);

			// IsValid
			if (entry.IsValid)
				this.FilesValid.Add(entry);

			// Album Artists
			if (entry.IsUnknown(x => x.AlbumArtists))
				this.AlbumArtistUnknown.Add(entry);

			// Album
			if (entry.IsUnknown(x => x.Album))
				this.AlbumUnknown.Add(entry);

			// Disc Count
			if (entry.IsUnknown(x => x.DiscCount))
				this.DiscCountUnknown.Add(entry);

			// Disc
			if (entry.IsUnknown(x => x.Disc))
				this.DiscUnknown.Add(entry);

			// Genres
			if (entry.IsUnknown(x => x.Genres))
				this.GenreUnknown.Add(entry);

			// Lyrics
			if (entry.IsUnknown(x => x.Lyrics))
				this.LyricsUnknown.Add(entry);

			// Title
			if (entry.IsUnknown(x => x.Title))
				this.TitleUnknown.Add(entry);

			// Track Count
			if (entry.IsUnknown(x => x.TrackCount))
				this.TrackCountUnknown.Add(entry);

			// Track
			if (entry.IsUnknown(x => x.Track))
				this.TrackUnknown.Add(entry);

			// Year
			if (entry.IsUnknown(x => x.Year))
				this.YearUnknown.Add(entry);
		}

		/// <summary>
		/// Clears all collections
		/// </summary>
		public void Clear()
		{
			this.FilesScanned.Clear();
			this.FilesValid.Clear();
			this.FilesEmpty.Clear();
			this.CompleteEntries.Clear();
			this.AlbumUnknown.Clear();
			this.AlbumArtistUnknown.Clear();
			this.GenreUnknown.Clear();
			this.LyricsUnknown.Clear();
			this.TitleUnknown.Clear();
			this.YearUnknown.Clear();
			this.TrackUnknown.Clear();
			this.TrackCountUnknown.Clear();
			this.DiscUnknown.Clear();
			this.DiscCountUnknown.Clear();
		}
	}
}
