using AudioPlayer.Component;
using AudioPlayer.Model.Interface;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AudioPlayer.Model.Database
{
    /// <summary>
    /// Contains the serialized contents of one library instance
    /// </summary>
    [Serializable]
    public class LibraryFile : ISerializable
    {
        // File Name <- 1-1 -> LibraryEntry
        // LibraryEntry <- N-1 -> Bitmap
        // Bitmaps stored separately


        // LibraryEntry.FileName -> LibraryEntry
        readonly Dictionary<string, ILibraryEntry> _entries;

        // LibraryEntry.ArtworkKey -> Artwork buffer (Bitmap)
        readonly Dictionary<string, SerializableBitmap> _artwork;

        public IEnumerable<ILibraryEntry> Entries
        {
            get { return _entries.Values; }
        }

        public IEnumerable<SerializableBitmap> Artwork
        {
            get { return _artwork.Values; }
        }

        public LibraryFile()
        {
            _entries = new Dictionary<string, ILibraryEntry>();
            _artwork = new Dictionary<string, SerializableBitmap>();
        }

        public LibraryFile(SerializationInfo info, StreamingContext context)
        {
            var entryCount = info.GetInt32("EntryCount");
            var artworkCount = info.GetInt32("ArtworkCount");

            _entries = new Dictionary<string, ILibraryEntry>();
            _artwork = new Dictionary<string, SerializableBitmap>();

            for (int i = 0; i < entryCount; i++)
            {
                var key = info.GetString("EntryKey" + i);
                var value = (ILibraryEntry)info.GetValue("EntryValue" + i, typeof(ILibraryEntry));

                _entries.Add(key, value);
            }

            for (int i = 0; i < artworkCount; i++)
            {
                var key = info.GetString("ArtworkKey" + i);
                var value = (SerializableBitmap)info.GetValue("ArtworkValue" + i, typeof(SerializableBitmap));

                _artwork.Add(key, value);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("EntryCount", _entries.Count);
            info.AddValue("ArtworkCount", _artwork.Count);

            var counter = 0;

            foreach (var element in _entries)
            {
                info.AddValue("EntryKey" + counter, element.Key);
                info.AddValue("EntryValue" + counter++, element.Value);
            }

            counter = 0;

            foreach (var element in _artwork)
            {
                info.AddValue("ArtworkKey" + counter, element.Key);
                info.AddValue("ArtworkValue" + counter++, element.Value);
            }
        }

        public bool ContainsArtworkFor(ILibraryEntry entry)
        {
            if (!LibraryArtworkLoader.HasValidArtworkKey(entry))
                return false;

            return _artwork.ContainsKey(LibraryArtworkLoader.GetArtworkKey(entry));
        }

        public SerializableBitmap GetArtwork(ILibraryEntry entry)
        {
            return _artwork[LibraryArtworkLoader.GetArtworkKey(entry)];
        }

        public void AddEntry(ILibraryEntry entry)
        {
            _entries.Add(entry.FileName, entry);
        }

        public void AddArtwork(string artworkKey, SerializableBitmap image)
        {
            if (!_artwork.ContainsKey(artworkKey))
            {
                _artwork.Add(artworkKey, image);
            }
        }
    }
}
