using Avalonia.Media.Imaging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        readonly Dictionary<string, LibraryEntry> _entries;

        // LibraryEntry.ArtworkKey -> Artwork buffer (Bitmap)
        readonly Dictionary<string, SerializableBitmap> _artwork;

        public IEnumerable<LibraryEntry> Entries
        {
            get { return _entries.Values; }
        }

        public IEnumerable<SerializableBitmap> Artwork
        {
            get { return _artwork.Values; }
        }

        public LibraryFile()
        {
            _entries = new Dictionary<string, LibraryEntry>();
            _artwork = new Dictionary<string, SerializableBitmap>();
        }

        public LibraryFile(SerializationInfo info, StreamingContext context)
        {
            var entryCount = info.GetInt32("EntryCount");
            var artworkCount = info.GetInt32("ArtworkCount");

            _entries = new Dictionary<string, LibraryEntry>();
            _artwork = new Dictionary<string, SerializableBitmap>();

            for (int i=0;i<entryCount;i++)
            {
                var key = info.GetString("EntryKey" + i);
                var value = (LibraryEntry)info.GetValue("EntryValue" + i, typeof(LibraryEntry));

                _entries.Add(key, value);
            }

            for (int i = 0; i < artworkCount; i++)
            {
                var key = info.GetString("ArtworkKey" + i);
                var value = (SerializableBitmap)info.GetValue("ArtworkValue" + i, typeof(SerializableBitmap));

                _artwork.Add(key, value);
            }

            OpenArtwork();
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

        /// <summary>
        /// Sets references to bitmaps in the Library Entry
        /// </summary>
        private void OpenArtwork()
        {
            foreach (var entry in _entries.Values)
            {
                if (_artwork.ContainsKey(entry.ArtworkKey))
                    entry.ArtworkResolved = _artwork[entry.ArtworkKey];
            }
        }

        public bool ContainsArtwork(LibraryEntry entry)
        {
            return _artwork.ContainsKey(entry.ArtworkKey);
        }

        public SerializableBitmap GetArtwork(string artworkKey)
        {
            return _artwork[artworkKey];
        }

        public void AddEntry(LibraryEntry entry)
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
