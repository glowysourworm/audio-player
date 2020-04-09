using AudioPlayer.Model;
using AudioPlayer.Model.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioPlayer.Component
{
    public static class MusicBrainzClient
    {
        public static IEnumerable<MusicBrainzRecord> Query(string artist, string album, string title)
        {
            return MusicBrainz.Search
                              .Recording(title, artist: artist, release: album)
                              .Data
                              .Select(result =>
            {
                return new MusicBrainzRecord()
                { 
                    
                };
            });
        }
    }
}
