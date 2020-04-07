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
        const int SCAN_CHUNK = 50;

        /// <summary>
        /// Occurs when files are ready to be added to the library. [ new entries, count completed, count total ]
        /// </summary>
        public event SimpleEventHandler<IEnumerable<LibraryEntry>, int, int> ScanUpdateEvent;

        // Background thread for scanning the directories
        Thread _worker;

        public FileScanner()
        {
            _worker = null;
        }

        public void Scan(string[] directories, string searchPattern = "*")
        {
            var files = new List<string>();

            // Gather file manifest for the scan
            foreach (var directory in directories)
                files.AddRange(Directory.GetFiles(directory, searchPattern, SearchOption.AllDirectories));

            // SCAN IN PROGRESS!
            if (_worker != null)
            {
                try
                {
                    _worker.Join(5000);
                    _worker = null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error joining scanning thread");

                    throw ex;
                }
            }

            // Begin the scan
            _worker = new Thread(() =>
            {
                ScanImpl(files.ToArray());
            });

            // USE HIGH PRIORITY TO FORCE DISK READS (PRETTY SURE) - THEN
            // THROTTLE USING THREAD.SLEEP(..)
            _worker.Priority = ThreadPriority.Highest;
            _worker.Start();
        }

        private void ScanImpl(string[] files)
        {
            // Procedure - NOTE*** Locking critical segments of code that can be done in a
            //                     small amount of time
            //
            // 1) Scan SCAN_CHUNK amount of files at once
            // 2) Foreach new file:
            //      - Read entry data
            //      - Update (or) Add entry
            //      - Update library statistics
            // 3) When SCAN_CHUNK number of files is finished - update main thread
            //

            var finishedEntries = new Dictionary<string, LibraryEntry>();

            // Begin scan of new entries
            for (int i = 0; i < files.Length; i++)
            {
                // ~ O(1)
                if (finishedEntries.ContainsKey(files[i]))
                    throw new Exception("Duplicate (invalid) file sent to library scan");

                // PERFORMANCE HIT - READ FILE TAG
                finishedEntries.Add(files[i], new LibraryEntry(files[i]));

                // UPDATE LISTENERS
                if (i % SCAN_CHUNK == 0 || i == files.Length - 1)
                {
                    // Update collections:  Send current finished entries to UI thread, then
                    //                      create new finished entries for this thread.
                    //

                    if (this.ScanUpdateEvent != null)
                        this.ScanUpdateEvent(finishedEntries.Values, i + 1, files.Length);

                    // USING HIGH PRIORITY THREAD + FORCED SLEEP TO SIMULATE BACKGROUND THREAD. This
                    // is done to get high priority disk access (at least on windows). The OS seems to
                    // de-prioritize reads to the disk by an application that is over-utilizing it. 
                    //
                    // Example: Drastic slowing down of reads after ~ 5000 tags.
                    //
                    // Solution:  Set thread priority High + allow thread sleep to let UI catch up
                    //            and (i think) to allow other applications to use the disk.
                    //
                    Thread.Sleep(10);

                    // Detach memory and start new collection
                    finishedEntries = new Dictionary<string, LibraryEntry>();
                }
            }
        }
    }
}
