using System;

namespace AudioPlayer.Model
{
	[Serializable]
	public class LibraryStatistics : ModelBase
	{
		// Valid = ??? + ??? + ???

		int _totalFilesScanned;
		int _totalFilesValid;
		int _totalFilesEmpty;

		int _totalCompleteEntries;

		int _totalAlbumUnknown;
		int _totalAlbumArtistUnknown;
		int _totalGenreUnknown;
		int _totalLyricsUnknown;
		int _totalTitleUnknown;
		int _totalYearUnknown;
		int _totalTrackUnknown;
		int _totalTrackCountUnknown;
		int _totalDiscUnknown;
		int _totalDiscCountUnknown;

		public int TotalFilesScanned
		{
			get { return _totalFilesScanned; }
			set { Update(ref _totalFilesScanned, value); }
		}
		public int TotalFilesValid
		{
			get { return _totalFilesValid; }
			set { Update(ref _totalFilesValid, value); }
		}
		public int TotalFilesEmpty
		{
			get { return _totalFilesEmpty; }
			set { Update(ref _totalFilesEmpty, value); }
		}
		public int TotalCompleteEntries
		{
			get { return _totalCompleteEntries; }
			set { Update(ref _totalCompleteEntries, value); }
		}
		public int TotalAlbumUnknown
		{
			get { return _totalAlbumUnknown; }
			set { Update(ref _totalAlbumUnknown, value); }
		}
		public int TotalAlbumArtistUnknown
		{
			get { return _totalAlbumArtistUnknown; }
			set { Update(ref _totalAlbumArtistUnknown, value); }
		}
		public int TotalGenreUnknown
		{
			get { return _totalGenreUnknown; }
			set { Update(ref _totalGenreUnknown, value); }
		}
		public int TotalLyricsUnknown
		{
			get { return _totalLyricsUnknown; }
			set { Update(ref _totalLyricsUnknown, value); }
		}
		public int TotalTitleUnknown
		{
			get { return _totalTitleUnknown; }
			set { Update(ref _totalTitleUnknown, value); }
		}
		public int TotalYearUnknown
		{
			get { return _totalYearUnknown; }
			set { Update(ref _totalYearUnknown, value); }
		}
		public int TotalTrackUnknown
		{
			get { return _totalTrackUnknown; }
			set { Update(ref _totalTrackUnknown, value); }
		}
		public int TotalTrackCountUnknown
		{
			get { return _totalTrackCountUnknown; }
			set { Update(ref _totalTrackCountUnknown, value); }
		}
		public int TotalDiscUnknown
		{
			get { return _totalDiscUnknown; }
			set { Update(ref _totalDiscUnknown, value); }
		}
		public int TotalDiscCountUnknown
		{
			get { return _totalDiscCountUnknown; }
			set { Update(ref _totalDiscCountUnknown, value); }
		}

		public LibraryStatistics() { }

		public void Clear()
		{
			this.TotalAlbumArtistUnknown = 0;
			this.TotalAlbumUnknown = 0;
			this.TotalCompleteEntries = 0;
			this.TotalDiscCountUnknown = 0;
			this.TotalDiscUnknown = 0;
			this.TotalFilesEmpty = 0;
			this.TotalFilesScanned = 0;
			this.TotalFilesValid = 0;
			this.TotalGenreUnknown = 0;
			this.TotalLyricsUnknown = 0;
			this.TotalTitleUnknown = 0;
			this.TotalTrackCountUnknown = 0;
			this.TotalTrackUnknown = 0;
			this.TotalYearUnknown = 0;
		}
	}
}
