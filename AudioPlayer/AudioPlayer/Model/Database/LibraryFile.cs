using Avalonia.Media.Imaging;

using System;
using System.Collections.Generic;
using System.IO;
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

        readonly Dictionary<string, LibraryEntry> _entries;
        readonly Dictionary<string, string> _artworkPointers;
        readonly Dictionary<string, byte[]> _artwork;

        public IEnumerable<LibraryEntry> Entries
        {
            get { return _entries.Values; }
        }

        public LibraryFile()
        {
            _entries = new Dictionary<string, LibraryEntry>();
            _artworkPointers = new Dictionary<string, string>();
            _artwork = new Dictionary<string, byte[]>();
        }

        public LibraryFile(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Entries", _entries);
            info.AddValue("ArtworkPointers", _artworkPointers);
            info.AddValue("Artwork", _artwork);

            OpenArtwork();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.GetValue("Entries", typeof(Dictionary<string, LibraryEntry>));
            info.GetValue("ArtworkPointers", typeof(Dictionary<string, string>));
            info.GetValue("Artwork", typeof(Dictionary<string, byte[]>));
        }

        /// <summary>
        /// Re-creates bitmaps from byte arrays and sets the reference in each LibraryEntry
        /// </summary>
        private void OpenArtwork()
        {
            foreach (var pointerKey in _artworkPointers.Keys)
            {
                var entry = _entries[pointerKey];
                var artworkBuffer = _artwork[_artworkPointers[pointerKey]];

                using (var stream = new MemoryStream(artworkBuffer))
                {
                    stream.Seek(0, SeekOrigin.Begin);

                    entry.ArtworkResolved = new Bitmap(stream);
                }
            }
        }

        public void AddEntry(LibraryEntry entry)
        {
            _entries.Add(entry.FileName, entry);
        }

        public void AddArtwork(LibraryEntry entry, Bitmap image)
        {
            string fingerPrint = "";

            var buffer = Serialize(image, out fingerPrint);

            if (!_artworkPointers.ContainsKey(entry.FileName))
                _artworkPointers.Add(entry.FileName, fingerPrint);

            if (!_artwork.ContainsKey(fingerPrint))
                _artwork.Add(fingerPrint, buffer);
        }

        private byte[] Serialize(Bitmap image, out string fingerprint)
        {
            using (var stream = new MemoryStream())
            {
                // Call Avalonia API
                image.Save(stream);

                // Fetch buffer from stream
                var buffer = stream.GetBuffer();

                // Compute MD5 Checksum -> Convert to string
                fingerprint = BitConverter.ToString(MD5.Create().ComputeHash(buffer));

                return buffer;
            }
        }
    }
}
