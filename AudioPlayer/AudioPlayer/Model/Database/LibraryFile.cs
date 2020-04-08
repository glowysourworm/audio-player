using Avalonia.Media.Imaging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;

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
        readonly Dictionary<string, Bitmap> _artwork;

        public IEnumerable<LibraryEntry> Entries
        {
            get { return _entries.Values; }
        }

        public LibraryFile()
        {
            _entries = new Dictionary<string, LibraryEntry>();
            _artwork = new Dictionary<string, Bitmap>();
        }

        public LibraryFile(SerializationInfo info, StreamingContext context)
        {
            _entries = (Dictionary<string, LibraryEntry>)info.GetValue("Entries", typeof(Dictionary<string, LibraryEntry>));

            // Deserialize artwork
            var artwork = (Dictionary<string, byte[]>)info.GetValue("Artwork", typeof(Dictionary<string, byte[]>));

            _artwork = artwork.ToDictionary(x => x.Key, x => Deserialize(x.Value));

            OpenArtwork();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Entries", _entries);

            // Serialize to Dictionary<string, byte[]>
            info.AddValue("Artwork", _artwork.ToDictionary(x => x.Key, x => Serialize(x.Value)));
        }

        /// <summary>
        /// Re-creates bitmaps from byte arrays and sets the reference in each LibraryEntry
        /// </summary>
        private void OpenArtwork()
        {
            foreach (var element in _artwork)
            {
                foreach (var entry in _entries.Values)
                {
                    if (entry.ArtworkKey == element.Key)
                        entry.ArtworkResolved = element.Value;
                }
            }
        }

        public bool ContainsArtwork(LibraryEntry entry)
        {
            return _artwork.ContainsKey(entry.ArtworkKey);
        }

        public Bitmap GetArtwork(string artworkKey)
        {
            return _artwork[artworkKey];
        }

        public void AddEntry(LibraryEntry entry)
        {
            _entries.Add(entry.FileName, entry);
        }

        public void AddArtwork(string artworkKey, Bitmap image)
        {
            if (!_artwork.ContainsKey(artworkKey))
            {
                _artwork.Add(artworkKey, image);
            }
        }

        private byte[] Serialize(Bitmap image)
        {
            using (var stream = new MemoryStream())
            {
                // Call Avalonia API
                image.Save(stream);

                // Fetch buffer from stream
                var buffer = stream.GetBuffer();

                return buffer;
            }
        }

        private Bitmap Deserialize(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                return new Bitmap(stream);
            }
        }
    }
}
