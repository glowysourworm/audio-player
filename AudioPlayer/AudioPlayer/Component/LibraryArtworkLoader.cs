using AudioPlayer.Extension;
using AudioPlayer.Model.Database;
using Avalonia.Media.Imaging;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayer.Component
{
    public static class LibraryArtworkLoader
    {
        /// <summary>
        /// Attempts to load album artwork from { tag pictures, album folder, web services }
        /// </summary>
        public static void Load(LibraryFile libraryFile)
        {
            // Create specific artwork keys for download
            var artworkGrouping = libraryFile.Entries
                                             .GroupBy(entry => entry.ArtworkKey)
                                             .Actualize();

            var artworkDict = new ConcurrentDictionary<string, Bitmap>();

            // Create tasks for downloading the artwork
            Parallel.ForEach(artworkGrouping, new ParallelOptions() { MaxDegreeOfParallelism = 8 }, group =>
            {
                // Tag Pictures (TODO)

                // Album Folder (TODO)

                // Web Services
                //
                // 1) LastFm
                // 2) ...
                //

                // Attemp to download artwork
                var artworkTask = LastFmClient.DownloadArtwork(group.First());

                // Run task
                artworkTask.Wait();

                // Add to artwork result
                if (artworkTask.Result != null)
                    artworkDict.TryAdd(group.Key, artworkTask.Result);
            });

            // Distribute results
            foreach (var element in artworkDict)
            {
                // Add distinct artwork to library by "Artwork Key"
                libraryFile.AddArtwork(element.Key, element.Value);

                // Set all references in the library file
                var group = artworkGrouping.FirstOrDefault(x => x.Key == element.Key);

                if (group != null)
                {
                    foreach (var entry in group)
                        entry.ArtworkResolved = libraryFile.GetArtwork(entry.ArtworkKey);
                }
            }
        }
    }
}
