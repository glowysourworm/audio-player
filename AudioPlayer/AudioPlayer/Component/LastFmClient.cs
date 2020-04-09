using AudioPlayer.Constant;
using AudioPlayer.Model;
using AudioPlayer.Model.Database;
using AudioPlayer.Model.Interface;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Enums;

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AudioPlayer.Component
{
    public static class LastFmClient
    {
        public static async Task<SerializableBitmap> DownloadArtwork(ILibraryEntry entry)
        {
            if (entry.IsUnknown(x => x.Album) ||
                entry.IsUnknown(x => x.AlbumArtists))
                return await Task.FromResult<SerializableBitmap>(null);

            else
            {
                try
                {
                    // Last FM API
                    var client = new LastfmClient(WebConfiguration.LastFmAPIKey, WebConfiguration.LastFmAPISecret);

                    // Web Call ...
                    var response = await client.Album.GetInfoAsync(entry.AlbumArtists.First(), entry.Album, false);

                    // Status OK -> Create bitmap image from the url
                    if (response.Status == LastResponseStatus.Successful &&
                        response.Content.Images.Any())
                        return await DownloadImage(response.Content.Images.ExtraLarge.AbsoluteUri);

                    else
                        return await Task.FromResult<SerializableBitmap>(null);
                }
                catch (Exception)
                {
                    return await Task.FromResult<SerializableBitmap>(null);
                }
            }
        }

        public static async Task<SerializableBitmap> DownloadImage(string imageUrl)
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

                        var bitmap = new SerializableBitmap(memoryStream);

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
