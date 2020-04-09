using AudioPlayer.Model.Database;

using System.Collections.Generic;

namespace AudioPlayer.Model.Interface
{
    public interface ILibraryMetaEntry
    {
        string Album { get; set; }
        string Title { get; set; }
        uint Year { get; set; }
        uint Track { get; set; }
        uint Disc { get; set; }
        uint DiscCount { get; set; }

        IEnumerable<string> AlbumArtists { get; set; }
        IEnumerable<string> Composers { get; set; }
        IEnumerable<string> Performers { get; set; }
        IEnumerable<string> Genres { get; set; }
    }
}
