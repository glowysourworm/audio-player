using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AudioPlayer.Event;
using AudioPlayer.Extension;
using AudioPlayer.Model;
using AudioPlayer.Model.Vendor;

namespace AudioPlayer.Component
{
    public static class MusicBrainzClient
    {
        /// <summary>
        /// Tries to look up information for the provided library entry
        /// </summary>
        public static Task<IEnumerable<MusicBrainzRecord>> Query(LibraryEntry entry)
        {
            return Task.Run<IEnumerable<MusicBrainzRecord>>(() =>
            {
                return MusicBrainz.Search
                              .Recording(entry.Title, artist: entry.AlbumArtists.FirstOrDefault(), release: entry.Album)
                              .Data
                              .Select(result => new
                              {
                                  Id = result.Id,
                                  Title = result.Title,
                                  Releases = result.Releaselist,
                                  Artists = result.Artistcredit,
                                  Score = result.Score
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
                                      MediumList = release.Mediumlist,
                                      Credits = release.Artistcredit,
                                      ReleaseEvents = release.Releaseeventlist,
                                      ReleaseGroup = release.Releasegroup,
                                      Score = x.Score,
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
                                  Score = x.Score,
                                  Year = x.Year
                              })
                              .OrderBy(x => x.Title)
                              .Actualize();
            });            
        }
    }
}
