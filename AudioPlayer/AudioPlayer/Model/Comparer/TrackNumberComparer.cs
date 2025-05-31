using System.Collections.Generic;

namespace AudioPlayer.Model.Comparer
{
    public class TrackNumberComparer : Comparer<LibraryEntry>
    {
        public override int Compare(LibraryEntry x, LibraryEntry y)
        {
            return x.Track.CompareTo(y.Track);
        }
    }
}
