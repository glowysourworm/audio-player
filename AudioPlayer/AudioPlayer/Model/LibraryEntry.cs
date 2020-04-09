using AudioPlayer.Extension;
using AudioPlayer.Model.Database;
using AudioPlayer.Model.Interface;
using ReactiveUI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

using TagLib;

namespace AudioPlayer.Model
{
    [Serializable]
    public class LibraryEntry : ModelBase, ILibraryEntry
    {
        #region (private) Backing Fields
        SortedObservableCollection<string, string> _albumArtists;
        SortedObservableCollection<string, string> _genres;

        string _fileName;
        string _album;
        string _title;
        uint _year;
        uint _track;
        uint _disc;
        uint _discCount;
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

        public LibraryEntry() { }

        public LibraryEntry(string file)
        {
            this.FileName = file;
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
            this.AlbumArtists = (SortedObservableCollection<string, string>)info.GetValue("AlbumArtists", typeof(SortedObservableCollection<string, string>));
            this.Genres = (SortedObservableCollection<string, string>)info.GetValue("Genres", typeof(SortedObservableCollection<string, string>));
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
        }
    }
}
