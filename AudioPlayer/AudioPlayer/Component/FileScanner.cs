using AudioPlayer.Event;
using AudioPlayer.Model;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace AudioPlayer.Component
{
    public class FileScanner
    {
        /// <summary>
        /// Returns the total number of files in the scan along with the entry
        /// </summary>
        public event SimpleEventHandler<LibraryEntry, int> FileScannedEvent;

        public FileScanner()
        {
        }

        public void Scan(string[] directories, string searchPattern = "*")
        {
            var files = new List<string>();

            // Gather file manifest for the scan
            foreach (var directory in directories)
                files.AddRange(Directory.GetFiles(directory, searchPattern, SearchOption.AllDirectories));

            // Queue separately on the thread pool
            foreach (var file in files)
            {
                ThreadPool.QueueUserWorkItem((parameters) =>
                {
                    var array = (object[])parameters;
                    var entry = new LibraryEntry((string)array[0]);
                    var count = (int)array[1];

                    if (this.FileScannedEvent != null)
                        this.FileScannedEvent(entry, count);

                }, new object[] { file, files.Count });
            }
        }
    }
}
