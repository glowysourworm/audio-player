using System.Collections.Generic;

using AudioPlayer.ViewModel.LibraryViewModel;

namespace AudioPlayer.Model.Comparer
{
    public class TrackNumberComparer : Comparer<TitleViewModel>
    {
        public override int Compare(TitleViewModel x, TitleViewModel y)
        {
            return x.Track.CompareTo(y.Track);
        }
    }
}
