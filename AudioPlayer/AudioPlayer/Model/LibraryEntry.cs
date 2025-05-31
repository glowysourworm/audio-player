using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

using AudioPlayer.Component;
using AudioPlayer.Event;
using AudioPlayer.Extension;
using AudioPlayer.Model.Command;
using AudioPlayer.Model.Database;
using AudioPlayer.Model.Vendor;
using AudioPlayer.ViewModel;

namespace AudioPlayer.Model
{
    [Serializable]
    public class LibraryEntry : ModelBase
    {
        public event SimpleEventHandler<string, LogMessageSeverity> LogEvent;

        #region (private) Backing Fields
        SortedObservableCollection<Artist> _albumArtists;
        SortedObservableCollection<string> _genres;
        SortedObservableCollection<SerializableBitmap> _albumArt;

        string _fileName;
        string _album;
        string _title;
        uint _year;
        uint _track;
        uint _disc;
        uint _discCount;
        TimeSpan _duration;

        // Music Brainz
        SortedObservableCollection<MusicBrainzRecord> _musicBrainzResults;            // Not Serialized (yet)
        MusicBrainzRecord _musicBrainzRecord;

        // Problems
        bool _fileMissing;
        bool _fileLoadError;
        bool _fileLocationNameMismatch;     // File folder path should be related to the album (see Tag loading)

        string _fileLoadErrorMessage;

        // Commands
        ModelCommand _queryMusicBrainzCommand;
        #endregion

        #region (public) Tag Fields
        public string FileName
        {
            get { return _fileName; }
            private set { SetProperty(ref _fileName, value); }
        }
        public string PrimaryArtist
        {
            get { return _albumArtists.FirstOrDefault()?.Name ?? string.Empty; }
        }
        public string Album
        {
            get { return _album; }
            set { SetProperty(ref _album, value); }
        }
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public uint Year
        {
            get { return _year; }
            set { SetProperty(ref _year, value); }
        }
        public uint Track
        {
            get { return _track; }
            set { SetProperty(ref _track, value); }
        }
        public uint Disc
        {
            get { return _disc; }
            set { SetProperty(ref _disc, value); }
        }
        public uint DiscCount
        {
            get { return _discCount; }
            set { SetProperty(ref _discCount, value); }
        }
        public TimeSpan Duration
        {
            get { return _duration; }
            set { this.SetProperty(ref _duration, value); }
        }

        public SortedObservableCollection<Artist> AlbumArtists
        {
            get { return _albumArtists; }
            set { SetProperty(ref _albumArtists, value); }
        }
        public SortedObservableCollection<SerializableBitmap> AlbumArt
        {
            get { return _albumArt; }
            set { this.SetProperty(ref _albumArt, value); }
        }
        public SortedObservableCollection<string> Genres
        {
            get { return _genres; }
            set { SetProperty(ref _genres, value); }
        }

        public bool FileMissing
        {
            get { return _fileMissing; }
            set { this.SetProperty(ref _fileMissing, value); }
        }
        public bool FileLoadError
        {
            get { return _fileLoadError; }
            set { this.SetProperty(ref _fileLoadError, value); }
        }
        public bool FileLocationNameMismatch
        {
            get { return _fileLocationNameMismatch; }
            set { this.SetProperty(ref _fileLocationNameMismatch, value); }
        }
        public string FileLoadErrorMessage
        {
            get { return _fileLoadErrorMessage; }
            set { this.SetProperty(ref _fileLoadErrorMessage, value); }
        }
        public SortedObservableCollection<MusicBrainzRecord> MusicBrainzResults
        {
            get { return _musicBrainzResults; }
            set { this.SetProperty(ref _musicBrainzResults, value); }
        }

        public MusicBrainzRecord MusicBrainzRecord
        {
            get { return _musicBrainzRecord; }
            set 
            { 
                this.SetProperty(ref _musicBrainzRecord, value);
                this.OnPropertyChanged("MusicBrainzRecordValid");
            }
        }
        public bool MusicBrainzRecordValid
        {
            get { return _musicBrainzRecord != MusicBrainzRecord.Empty; }
        }


        public ModelCommand QueryMusicBrainzCommand
        {
            get { return _queryMusicBrainzCommand; }
            set { this.SetProperty(ref _queryMusicBrainzCommand, value); }
        }

        #endregion

        public LibraryEntry() { }

        public LibraryEntry(string file)
        {
            this.AlbumArt = new SortedObservableCollection<SerializableBitmap>();
            this.AlbumArtists = new SortedObservableCollection<Artist>();
            this.FileName = file;
            this.MusicBrainzRecord = MusicBrainzRecord.Empty;
            this.MusicBrainzResults = new SortedObservableCollection<MusicBrainzRecord>();

            this.MusicBrainzRecord.PropertyChanged += MusicBrainzRecord_PropertyChanged;

            this.OnPropertyChanged("MusicBrainzRecordValid");   // Calculated property (initialize)

            this.QueryMusicBrainzCommand = new ModelCommand(async () =>
            {
                await QueryMusicBrainz();
            });
        }

        ~LibraryEntry()
        {
            this.MusicBrainzRecord.PropertyChanged -= MusicBrainzRecord_PropertyChanged;
        }

        private void MusicBrainzRecord_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Dependent Properties
            this.OnPropertyChanged("MusicBrainzRecord");        // Bubble up property changes
            this.OnPropertyChanged("MusicBrainzRecordValid");   // Calculated property
        }

        private async Task QueryMusicBrainz()
        {
            try
            {
                OnLog("Querying MusicBrainz (remote): Title:  {0}", LogMessageSeverity.Info, this.Title);

                // Music Brainz!
                //
                var records = await MusicBrainzClient.Query(this);

                // TODO: Need Best Record Matcher
                this.MusicBrainzRecord = records.FirstOrDefault() ?? MusicBrainzRecord.Empty;

                foreach (var record in records)
                {
                    this.MusicBrainzResults.Add(record);
                }

                OnLog("MusicBrainz Query Finished: {0} Results (arranged by score)", LogMessageSeverity.Info, records.Count());
            }
            catch (Exception ex)
            {
                OnLog("Music Brainz Error:  {0}", LogMessageSeverity.Error, ex.Message);
            }
        }

        private void OnLog(string message, LogMessageSeverity severity, params object[] args)
        {
            if (this.LogEvent != null)
                this.LogEvent(string.Format(message, args), severity);
        }

        public LibraryEntry(SerializationInfo info, StreamingContext context)
        {
            this.FileName = info.GetString("FileName");

            this.Album = info.GetString("Album");
            this.Title = info.GetString("Title");
            this.Year = info.GetUInt32("Year");
            this.Track = info.GetUInt32("Track");
            this.Disc = info.GetUInt32("Disc");
            this.DiscCount = info.GetUInt32("DiscCount");
            this.AlbumArtists = (SortedObservableCollection<Artist>)info.GetValue("AlbumArtists", typeof(SortedObservableCollection<Artist>));
            this.Genres = (SortedObservableCollection<string>)info.GetValue("Genres", typeof(SortedObservableCollection<string>));

            this.FileMissing = info.GetBoolean("FileMissing");
            this.FileLoadError = info.GetBoolean("FileLoadError");
            this.FileLocationNameMismatch = info.GetBoolean("FileLocationNameMismatch");
            this.FileLoadErrorMessage = info.GetString("FileLoadErrorMessage");

            this.MusicBrainzRecord = (MusicBrainzRecord)info.GetValue("MusicBrainzRecord", typeof(MusicBrainzRecord));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("FileName", this.FileName);

            info.AddValue("Album", this.Album);
            info.AddValue("Title", this.Title);
            info.AddValue("Year", this.Year);
            info.AddValue("Track", this.Track);
            info.AddValue("Disc", this.Disc);
            info.AddValue("DiscCount", this.DiscCount);
            info.AddValue("AlbumArtists", this.AlbumArtists);
            info.AddValue("Genres", this.Genres);

            info.AddValue("FileMissing", this.FileMissing);
            info.AddValue("FileLoadError", this.FileLoadError);
            info.AddValue("FileLocationNameMismatch", this.FileLocationNameMismatch);
            info.AddValue("FileLoadErrorMessage", this.FileLoadErrorMessage);

            info.AddValue("MusicBrainzRecord", this.MusicBrainzRecord);
        }
    }
}
