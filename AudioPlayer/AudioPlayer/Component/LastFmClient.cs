using AudioPlayer.Constant;
using AudioPlayer.Model;

using Avalonia.Media.Imaging;

using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Enums;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AudioPlayer.Component
{
    public static class LastFmClient
    {
        public static async Task<Bitmap> DownloadArtwork(LibraryEntry entry)
        {
            if (entry.IsUnknown(x => x.Album) ||
                entry.IsUnknown(x => x.AlbumArtists))
                return await Task.FromResult<Bitmap>(null);

            else
            {
                try
                {
                    // Last FM API
                    var client = new LastfmClient(WebConfiguration.LastFmAPIKey, WebConfiguration.LastFmAPISecret);

                    // Web Call ...
                    var response = await client.Album.GetInfoAsync(entry.AlbumArtists[0], entry.Album, true);

                    // Status OK -> Create bitmap image from the url
                    if (response.Status == LastResponseStatus.Successful)
                        return await DownloadImage(response.Content.Images.ExtraLarge.AbsoluteUri);

                    else
                        return await Task.FromResult<Bitmap>(null);
                }
                catch (Exception)
                {
                    return await Task.FromResult<Bitmap>(null);
                }
            }
        }

        public static async Task<Bitmap> DownloadImage(string imageUrl)
        {
            try
            {
                var request = WebRequest.Create(imageUrl);
                var response = await request.GetResponseAsync();

                // NOTE*** ISSUE WITH AVALONIA UI BITMAP CONSTRUCTOR - HAD TO USE MEMORY STREAM
                using (var stream = response.GetResponseStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);

                        memoryStream.Seek(0, SeekOrigin.Begin);

                        var bitmap = new Bitmap(memoryStream);

                        return bitmap;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
