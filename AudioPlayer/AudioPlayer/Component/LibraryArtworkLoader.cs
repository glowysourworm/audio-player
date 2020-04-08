using AudioPlayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AudioPlayer.Component
{
    public static class LibraryArtworkLoader
    {
        /// <summary>
        /// Attempts to load album artwork from { tag pictures, album folder, web services }
        /// </summary>
        public static void Load(Library library)
        {
            foreach (var entry in library.AllTitles)
            {
                ThreadPool.QueueUserWorkItem(async (parameters) =>
                {
                    Thread.CurrentThread.Priority = ThreadPriority.Lowest;

                    var array = (object[])parameters;
                    var entry = (LibraryEntry)array[0];

                    // Tag Pictures (TODO)

                    // Album Folder (TODO)

                    // Web Services
                    //
                    // 1) LastFm
                    // 2) ...
                    //

                    var image = await LastFmClient.DownloadArtwork(entry);

                    if (image != null)
                        entry.ArtworkResolved = image;

                    await Task.Delay(5);

                }, new object[] { entry });
            }
        }
    }
}
