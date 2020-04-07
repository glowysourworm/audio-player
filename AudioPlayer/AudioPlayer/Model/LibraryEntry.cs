using AudioPlayer.Extension;
using ReactiveUI;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace AudioPlayer.Model
{
    [Serializable]
    public class LibraryEntry : ModelBase
    {
        const string UNKNOWN = "[Unknown]";

        string _fileName;

        public SortedObservableCollection<string, string> AlbumArtists { get; set; }
        public SortedObservableCollection<string, string> Composers { get; set; }
        public SortedObservableCollection<string, string> Performers { get; set; }
        public SortedObservableCollection<string, string> Genres { get; set; }

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

        // CALCULATED FIELDS
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

        public LibraryEntry() { this.IsValid = false; }

        public LibraryEntry(string file)
        {
            Open(file);
        }

        public LibraryEntry(LibraryEntry copy)
        {
            this.FileName = copy.FileName;

            this.AlbumArtists = new SortedObservableCollection<string, string>(copy.AlbumArtists, x => x, true);
            this.Composers = new SortedObservableCollection<string, string>(copy.Composers, x => x, true);
            this.Genres = new SortedObservableCollection<string, string>(copy.Genres, x => x, true);
            this.Performers = new SortedObservableCollection<string, string>(copy.Performers, x => x, true);

            this.Album = copy.Album;
            this.BeatsPerMinute = copy.BeatsPerMinute;
            this.Comment = copy.Comment;
            this.Conductor = copy.Conductor;
            this.Copyright = copy.Copyright;
            this.Disc = copy.Disc;
            this.DiscCount = copy.DiscCount;
            this.FileName = copy.FileName;
            this.Grouping = copy.Grouping;
            this.IsEmpty = copy.IsEmpty;
            this.IsValid = copy.IsValid;
            this.Lyrics = copy.Lyrics;
            this.Title = copy.Title;
            this.Track = copy.Track;
            this.TrackCount = copy.TrackCount;
            this.Year = copy.Year;

            this.RaisePropertyChanged("AlbumArtistsJoined");
            this.RaisePropertyChanged("GenresJoined");
        }

        public bool IsUnknown<T>(Expression<Func<LibraryEntry, T>> propertyExpression)
        {
            var expression = (MemberExpression)propertyExpression.Body;
            var propertyInfo = (PropertyInfo)expression.Member;
            var propertyValue = propertyInfo.GetValue(this);

            // String
            if (propertyValue is string)
            {
                if (string.IsNullOrWhiteSpace((string)propertyValue))
                    return true;

                else if ((string)propertyValue == UNKNOWN)
                    return true;

                else
                    return false;
            }

            // Collections of strings
            else if (propertyValue is SortedObservableCollection<string, string>)
            {
                var collection = propertyValue as SortedObservableCollection<string, string>;

                return collection.Count == 0;
            }

            // uint
            else if (propertyValue is uint)
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

                this.RaisePropertyChanged("AlbumArtistsJoined");
                this.RaisePropertyChanged("GenresJoined");
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
    }
}
