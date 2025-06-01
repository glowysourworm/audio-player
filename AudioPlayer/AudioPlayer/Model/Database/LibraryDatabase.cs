using System;
using System.Collections.Generic;

using AudioPlayer.Component;

namespace AudioPlayer.Model.Database
{
    /// <summary>
    /// Represents library entries and other serialized data for the library. This is separate to
    /// the Configuration (serialized) object.
    /// </summary>
    [Serializable]
    public class LibraryDatabase
    {
        public List<LibraryEntry> Entries { get; set; }

        public LibraryDatabase()
        {
            this.Entries = new List<LibraryEntry>();
        }
        public LibraryDatabase(Library library)
        {
            this.Entries = new List<LibraryEntry>(library.AllTitles);
        }
        public LibraryDatabase(IEnumerable<LibraryEntry> entries)
        {
            this.Entries = new List<LibraryEntry>(entries);
        }

        /// <summary>
        /// Creates library object from loaded data
        /// </summary>
        public Library CreateLibrary()
        {
            var library = new Library();

            foreach (var entry in this.Entries)
                library.Add(entry);

            return library;
        }

        public static LibraryDatabase Open(string fileName)
        {
            try
            {
                return Serializer.Deserialize<LibraryDatabase>(fileName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Saves the database to file. Returns success / failure.
        /// </summary>
        public bool Save(string fileName)
        {
            try
            {
                Serializer.Serialize(this, fileName);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
