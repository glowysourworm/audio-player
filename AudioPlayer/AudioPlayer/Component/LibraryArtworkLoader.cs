using AudioPlayer.Extension;
using AudioPlayer.Model.Database;

using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayer.Component
{
    public static class LibraryArtworkLoader
    {
        /// <summary>
        /// Attempts to load album artwork from { tag pictures, album folder, web services }
        /// </summary>
        public static async Task Load(LibraryFile libraryFile)
        {
            // Create specific artwork keys for download
            var artworkGrouping = libraryFile.Entries
                                             .GroupBy(entry => entry.ArtworkKey)
                                             .Actualize();

            // Create tasks for downloading the artwork
            var tasks = artworkGrouping.Select(async group =>
            {
                // Tag Pictures (TODO)

                // Album Folder (TODO)

                // Web Services
                //
                // 1) LastFm
                // 2) ...
                //

                // Attemp to download artwork
                var artwork = await LastFmClient.DownloadArtwork(group.First());

                if (artwork != null)
                {
                    // Add distinct artwork to library by "Artwork Key"
                    libraryFile.AddArtwork(group.Key, artwork);

                    // Set all references in the group
                    foreach (var entry in group)
                        entry.ArtworkResolved = libraryFile.GetArtwork(entry.ArtworkKey);
                }
            });

            // Finish web service calls
            await Task.WhenAll(tasks);
        }
    }
}
