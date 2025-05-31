using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AudioPlayer.Extensions.NativeIO.FastDirectory;
using AudioPlayer.Model;
using AudioPlayer.Model.EventHandler;
using AudioPlayer.ViewModel;

namespace AudioPlayer.Component
{
    public static class LibraryLoader
    {
        /// <summary>
        /// Loads .mp3 file entries from file and creates a library from the entries
        /// </summary>
        public static Task<IEnumerable<LibraryEntry>> Load(LibraryConfiguration configuration, SimpleHandler<string, LogMessageSeverity> messageHandler)
        {
            return Task<IEnumerable<LibraryEntry>>.Run<IEnumerable<LibraryEntry>>(() =>
            {
                // Scan directories for files (Use NativeIO for much faster iteration. Less managed memory loading)
                var files = FastDirectoryEnumerator.GetFiles(configuration.DirectoryBase, "*.mp3", SearchOption.AllDirectories);

                var entries = new ConcurrentBag<LibraryEntry>();

                // Use TPL to create library entries
                Parallel.ForEach(files, new ParallelOptions() { MaxDegreeOfParallelism = 4 }, (file) =>
                {
                    LibraryEntry entry = null;

                    // Generate entry
                    try
                    {
                        entry = LibraryEntryLoader.Load(file.Path);

                        messageHandler(string.Format("Music file loaded:  {0}", file.Path), LogMessageSeverity.Info);
                    }
                    catch (Exception ex)
                    {
                        entry = new LibraryEntry(file.Path)
                        {
                            FileLoadError = true,
                            FileLoadErrorMessage = ex.Message,        // Should be handled based on exception (user friendly message)
                        };

                        messageHandler(string.Format("Music file load error:  {0}", file.Path), LogMessageSeverity.Error);
                    }

                    // Keep track of completed entries 
                    if (entry != null)
                        entries.Add(entry);

                    else
                        throw new Exception("Unhandled library loader exception:  LibraryLoader.cs");
                });

                // Report
                messageHandler(string.Format("{0} music files read successfully! {1} of {2} loaded. {3} had loading issues.", 
                               entries.Count, 
                               entries.Where(x => !x.FileLoadError).Count(),
                               entries.Count,
                               entries.Where(x => x.FileLoadError).Count()),
                               entries.Where(x => x.FileLoadError).Count() == 0 ? LogMessageSeverity.Info : LogMessageSeverity.Error);

                messageHandler("See Library Manager to resolve loading issues", LogMessageSeverity.Info);

                return entries;

            });
        }
    }
}
