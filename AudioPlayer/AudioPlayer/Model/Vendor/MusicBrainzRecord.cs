using AudioPlayer.Extension;
using AudioPlayer.Model.Interface;

using System.Collections.Generic;

namespace AudioPlayer.Model.Vendor
{
    /// <summary>
    /// Extracted data from music brainz query
    /// </summary>
    public class MusicBrainzRecord : ModelBase, ILibraryMetaEntry
    {
        #region (private) Backing Fields
        SortedObservableCollection<string, string> _albumArtists;
        SortedObservableCollection<string, string> _genres;

        string _musicBrainzRecordingId;
        string _musicBrainzReleaseCountry;
        string _musicBrainzReleaseStatus;

        string _album;
        string _title;
        uint _year;
        uint _track;
        uint _disc;
        uint _discCount;
        #endregion

        #region (public) Tag + MusicBrainz Fields
        public string MusicBrainzRecordingId
        {
            get { return _musicBrainzRecordingId; }
            private set { Update(ref _musicBrainzRecordingId, value); }
        }
        public string MusicBrainzReleaseCountry
        {
            get { return _musicBrainzReleaseCountry; }
            set { Update(ref _musicBrainzReleaseCountry, value); }
        }
        public string MusicBrainzReleaseStatus
        {
            get { return _musicBrainzReleaseStatus; }
            set { Update(ref _musicBrainzReleaseStatus, value); }
        }
        public string Album
        {
            get { return _album; }
            set { Update(ref _album, value); }
        }
        public string Title
        {
            get { return _title; }
            set { Update(ref _title, value); }
        }
        public uint Year
        {
            get { return _year; }
            set { Update(ref _year, value); }
        }
        public uint Track
        {
            get { return _track; }
            set { Update(ref _track, value); }
        }
        public uint Disc
        {
            get { return _disc; }
            set { Update(ref _disc, value); }
        }
        public uint DiscCount
        {
            get { return _discCount; }
            set { Update(ref _discCount, value); }
        }

        public SortedObservableCollection<string, string> AlbumArtists
        {
            get { return _albumArtists; }
            set { Update(ref _albumArtists, value); }
        }
        public SortedObservableCollection<string, string> Genres
        {
            get { return _genres; }
            set { Update(ref _genres, value); }
        }
        #endregion

        public MusicBrainzRecord(string recordingId)
        {
            this.MusicBrainzRecordingId = recordingId;
        }
    }
}
