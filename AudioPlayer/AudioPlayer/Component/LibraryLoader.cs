using AudioPlayer.Extension;
using AudioPlayer.Model;
using AudioPlayer.Model.Database;

using Avalonia.Threading;

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayer.Component
{
    public static class LibraryLoader
    {
        public static async Task<LibraryFile> Load(string[] directories)
        {
            var libraryFile = new LibraryFile();

            // Scan directories for files
            var files = directories.SelectMany(directory =>
            {
                return Directory.GetFiles(directory, "*.mp3", SearchOption.AllDirectories);

            }).ToList();

            var entries = new ConcurrentBag<LibraryEntry>();

            // Use TPL to create library entries
            Parallel.ForEach(files, new ParallelOptions() { MaxDegreeOfParallelism = 4 }, (file) =>
            {
                // Generate entry
                var entry = new LibraryEntry(file);

                // Keep track of completed entries 
                entries.Add(entry);
            });

            // Signal UI Thread with update
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                foreach (var entry in entries)
                {
                    // Add entry to the library
                    //
                    // TODO: HANDLE INVALID ENTRIES
                    if (entry.IsValid)
                        libraryFile.AddEntry(entry);
                }
            });

            return libraryFile;
        }

        public static LibraryFile Load(string libraryFileName)
        {
            try
            {
                // Load library database
                return Serializer.Deserialize<LibraryFile>(libraryFileName);
            }
            catch (Exception)
            {
                return new LibraryFile();
            }
        }
    }
}
