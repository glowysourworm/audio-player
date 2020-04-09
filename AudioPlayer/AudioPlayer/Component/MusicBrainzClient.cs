using AudioPlayer.Extension;
using AudioPlayer.Model.Interface;
using AudioPlayer.Model.Vendor;

using System.Collections.Generic;
using System.Linq;

namespace AudioPlayer.Component
{
    public static class MusicBrainzClient
    {
        /// <summary>
        /// Tries to look up information for the provided library entry
        /// </summary>
        public static IEnumerable<MusicBrainzRecord> Query(ILibraryEntry entry)
        {
            return MusicBrainz.Search
                              .Recording(entry.Title, artist: entry.AlbumArtists.FirstOrDefault(), release: entry.Album)
                              .Data
                              .Select(result => new
                              {
                                  Id = result.Id,
                                  Title = result.Title,
                                  Releases = result.Releaselist,
                                  Artists = result.Artistcredit
                              })
                              .SelectMany(x => x.Releases.Select(release =>
                              {
                                  uint year = 0;
                                  if (!uint.TryParse(release.Date, out year))
                                      year = 0;

                                  return new
                                  {
                                      Id = x.Id,
                                      Title = x.Title,
                                      Release = release.Title,
                                      Track = release.Mediumlist.Medium.Position,
                                      Country = release.Country,
                                      Status = release.Status,
                                      Year = year,
                                      Artists = x.Artists.Select(artist => artist.Artist.Sortname)
                                  };
                              }))
                              .Select(x => new MusicBrainzRecord(x.Id)
                              {
                                  Album = x.Release,
                                  AlbumArtists = new SortedObservableCollection<string, string>(x.Artists, x => x),
                                  MusicBrainzReleaseCountry = x.Country,
                                  MusicBrainzReleaseStatus = x.Status,
                                  Title = x.Title,
                                  Track = (uint)x.Track,
                                  Year = x.Year
                              })
                              .OrderBy(x => x.Title)
                              .Actualize();
        }
    }
}
