using AudioPlayer.Extension;

using Avalonia.Media.Imaging;

using ReactiveUI;

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

using TagLib;

namespace AudioPlayer.Model
{
    [Serializable]
    public class LibraryEntry : ModelBase, ISerializable
    {
        const string UNKNOWN = "Unknown";

        #region (private) Backing Fields
        string _fileName;

        string _album;
        string _comment;
        string _conductor;
        string _copyright;
        string _grouping;
        string _lyrics;
        string _title;

        uint _year;
        uint _track;
        uint _trackCount;
        uint _disc;
        uint _discCount;
        uint _beatsPerMinute;

        bool _isEmpty; // tag field
        bool _isValid;

        Tag _originalTag;
        Bitmap _artworkResolved;
        #endregion

        #region (public) Tag Fields
        public string FileName
        {
            get { return _fileName; }
            private set { Update(ref _fileName, value); }
        }

        public string Album
        {
            get { return _album; }
            set { Update(ref _album, value); }
        }
        public string Comment
        {
            get { return _comment; }
            set { Update(ref _comment, value); }
        }
        public string Conductor
        {
            get { return _conductor; }
            set { Update(ref _conductor, value); }
        }
        public string Copyright
        {
            get { return _copyright; }
            set { Update(ref _copyright, value); }
        }
        public string Grouping
        {
            get { return _grouping; }
            set { Update(ref _grouping, value); }
        }
        public string Lyrics
        {
            get { return _lyrics; }
            set { Update(ref _lyrics, value); }
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
        public uint TrackCount
        {
            get { return _trackCount; }
            set { Update(ref _trackCount, value); }
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
        public uint BeatsPerMinute
        {
            get { return _beatsPerMinute; }
            set { Update(ref _beatsPerMinute, value); }
        }

        public bool IsEmpty
        {
            get { return _isEmpty; }
            set { Update(ref _isEmpty, value); }
        }

        public SortedObservableCollection<string, string> AlbumArtists { get; set; }
        public SortedObservableCollection<string, string> Composers { get; set; }
        public SortedObservableCollection<string, string> Performers { get; set; }
        public SortedObservableCollection<string, string> Genres { get; set; }
        #endregion

        #region (public) Calculated Properties
        public string AlbumArtistsJoined
        {
            get { return Format(string.Join(';', this.AlbumArtists)); }
        }
        public string GenresJoined
        {
            get { return Format(string.Join(';', this.Genres)); }
        }
        public bool IsValid
        {
            get { return _isValid; }
            set { Update(ref _isValid, value); }
        }
        public bool IsComplete
        {
            // TODO: Make these fields configurable
            get
            {
                return !IsUnknown(x => x.Album) &&
                       !IsUnknown(x => x.AlbumArtists) &&
                       !IsUnknown(x => x.Disc) &&
                       !IsUnknown(x => x.DiscCount) &&
                       !IsUnknown(x => x.Lyrics) &&
                       !IsUnknown(x => x.Title) &&
                       !IsUnknown(x => x.Track) &&
                       !IsUnknown(x => x.Year);
            }
        }

        /// <summary>
        /// TODO: CHANGE THIS - Used to share artwork for an album. This needs a better
        ///       unique ID.
        /// </summary>
        public string ArtworkKey
        {
            get { return this.Album + this.AlbumArtistsJoined; }
        }

        /// <summary>
        /// Artwork resolved from one of any valid resource { tag pictures, album folder, web services }
        /// </summary>
        public Bitmap ArtworkResolved
        {
            get { return _artworkResolved; }
            set { Update(ref _artworkResolved, value); }
        }
        #endregion

        #region (public) API Properties
        public Tag OriginalTag
        {
            get { return _originalTag; }
            set { Update(ref _originalTag, value); }
        }
        #endregion

        public LibraryEntry() { this.IsValid = false; }

        public LibraryEntry(string file)
        {
            Open(file);
        }

        public LibraryEntry(SerializationInfo info, StreamingContext context)
        {
            this.FileName = info.GetString("FileName");

            this.AlbumArtists = (SortedObservableCollection<string, string>)info.GetValue("AlbumArtists", typeof(SortedObservableCollection<string, string>));
            this.Composers = (SortedObservableCollection<string, string>)info.GetValue("Composers", typeof(SortedObservableCollection<string, string>));
            this.Genres = (SortedObservableCollection<string, string>)info.GetValue("Genres", typeof(SortedObservableCollection<string, string>));
            this.Performers = (SortedObservableCollection<string, string>)info.GetValue("Performers", typeof(SortedObservableCollection<string, string>));

            this.Album = info.GetString("Album");
            this.BeatsPerMinute = info.GetUInt32("BeatsPerMinute");
            this.Comment = info.GetString("Comment");
            this.Conductor = info.GetString("Conductor");
            this.Copyright = info.GetString("Copyright");
            this.Disc = info.GetUInt32("Disc");
            this.DiscCount = info.GetUInt32("DiscCount");
            this.Grouping = info.GetString("Grouping");
            this.IsEmpty = info.GetBoolean("IsEmpty");
            this.IsValid = info.GetBoolean("IsValid");
            this.Lyrics = info.GetString("Lyrics");
            this.Title = info.GetString("Title");
            this.Track = info.GetUInt32("Track");
            this.TrackCount = info.GetUInt32("TrackCount");
            this.Year = info.GetUInt32("Year");

            try
            {
                this.OriginalTag = TagLib.File.Create(this.FileName).Tag;
            }
            catch (Exception)
            {
                this.OriginalTag = null;
                this.IsValid = false;
            }

            OnCalculatedFieldsChanged();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("FileName", this.FileName);

            info.AddValue("AlbumArtists", this.AlbumArtists);
            info.AddValue("Composers", this.Composers);
            info.AddValue("Genres", this.Genres);
            info.AddValue("Performers", this.Performers);

            info.AddValue("Album", this.Album);
            info.AddValue("BeatsPerMinute", this.BeatsPerMinute);
            info.AddValue("Comment", this.Comment);
            info.AddValue("Conductor", this.Conductor);
            info.AddValue("Copyright", this.Copyright);
            info.AddValue("Disc", this.Disc);
            info.AddValue("DiscCount", this.DiscCount);
            info.AddValue("Grouping", this.Grouping);
            info.AddValue("IsEmpty", this.IsEmpty);
            info.AddValue("IsValid", this.IsValid);
            info.AddValue("Lyrics", this.Lyrics);
            info.AddValue("Title", this.Title);
            info.AddValue("Track", this.Track);
            info.AddValue("TrackCount", this.TrackCount);
            info.AddValue("Year", this.Year);
        }

        public bool IsUnknown<T>(Expression<Func<LibraryEntry, T>> propertyExpression)
        {
            var expression = (MemberExpression)propertyExpression.Body;
            var propertyInfo = (PropertyInfo)expression.Member;
            var propertyValue = propertyInfo.GetValue(this);

            // String
            if (propertyInfo.PropertyType == typeof(string))
            {
                if (string.IsNullOrWhiteSpace((string)propertyValue))
                    return true;

                else if ((string)propertyValue == UNKNOWN)
                    return true;

                else
                    return false;
            }

            // Collections of strings
            else if (propertyInfo.PropertyType == typeof(SortedObservableCollection<string, string>))
            {
                var collection = propertyValue as SortedObservableCollection<string, string>;

                return collection.Count == 0;
            }

            // uint
            else if (propertyInfo.PropertyType == typeof(uint))
            {
                return (uint)propertyValue <= 0;
            }

            else
                throw new Exception("Unhandled unknown field type LibraryEntry.IsUnknown");
        }

        private void Open(string file)
        {
            if (string.IsNullOrEmpty(file))
                throw new ArgumentException("Invalid media file name");

            this.FileName = file;

            try
            {
                var fileRef = TagLib.File.Create(file);

                this.AlbumArtists = new SortedObservableCollection<string, string>(fileRef.Tag.AlbumArtists, x => x, true);
                this.Composers = new SortedObservableCollection<string, string>(fileRef.Tag.Composers, x => x, true);
                this.Genres = new SortedObservableCollection<string, string>(fileRef.Tag.Genres, x => x, true);
                this.Performers = new SortedObservableCollection<string, string>(fileRef.Tag.Performers, x => x, true);

                this.Album = Format(fileRef.Tag.Album);
                this.BeatsPerMinute = fileRef.Tag.BeatsPerMinute;
                this.Comment = Format(fileRef.Tag.Comment);
                this.Conductor = Format(fileRef.Tag.Conductor);
                this.Copyright = Format(fileRef.Tag.Copyright);
                this.Disc = fileRef.Tag.Disc;
                this.DiscCount = fileRef.Tag.DiscCount;
                this.Grouping = Format(fileRef.Tag.Grouping);
                this.IsEmpty = fileRef.Tag.IsEmpty;
                this.Lyrics = Format(fileRef.Tag.Lyrics);
                this.Title = Format(fileRef.Tag.Title);
                this.Track = fileRef.Tag.Track;
                this.TrackCount = fileRef.Tag.TrackCount;
                this.Year = fileRef.Tag.Year;

                this.IsValid = true;

                // this.OriginalTag = fileRef.Tag;

                OnCalculatedFieldsChanged();
            }
            catch (Exception)
            {
                this.IsValid = false;
            }
        }

        private string Format(string tagField)
        {
            return string.IsNullOrWhiteSpace(tagField) ? UNKNOWN : tagField;
        }

        private void OnCalculatedFieldsChanged()
        {
            this.RaisePropertyChanged("AlbumArtistsJoined");
            this.RaisePropertyChanged("GenresJoined");
            this.RaisePropertyChanged("IsComplete");
            this.RaisePropertyChanged("ArtworkKey");
        }
    }
}
