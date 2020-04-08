using AudioPlayer.Extension;
using AudioPlayer.Model;
using AudioPlayer.Model.Database;

using Avalonia.Threading;

using System;
using System.Linq;
using System.Threading;

namespace AudioPlayer.Component
{
    public static class LibraryLoader
    {
        public static Library Load(string[] directories, Action<string> statusUpdate)
        {
            var library = new Library();
            var scanner = new FileScanner();

            // Create new library directories
            library.Directories.ReCreate(directories.Distinct());

            var counter = 0;

            scanner.FileScannedEvent += (entry, count) =>
            {
                // Atomic increment
                Interlocked.Increment(ref counter);

                if (!entry.IsValid)
                    return;

                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    library.Add(entry);

                    statusUpdate(string.Format("Scanned {0} of {1} files...", counter, count));

                }, DispatcherPriority.MaxValue);
            };

            scanner.Scan(directories, "*.mp3");

            return library;
        }

        public static Library Load(string libraryFileName)
        {
            try
            {
                // Load library database
                var libraryFile = Serializer.Deserialize<LibraryFile>(libraryFileName);

                // Load library from the database
                return new Library(libraryFile);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string GetScanStatus(int completed, int total)
        {
            return string.Format("({0} of {1}) files scanned...", completed, total);
        }
    }
}
