using System.Collections.Generic;

using AudioPlayer.Model.Vendor;

namespace AudioPlayer.Model.Comparer
{
    public class MusicBrainzScoreComparer : Comparer<MusicBrainzRecord>
    {
        public override int Compare(MusicBrainzRecord x, MusicBrainzRecord y)
        {
            return x.Score.CompareTo(y.Score);
        }
    }
}
