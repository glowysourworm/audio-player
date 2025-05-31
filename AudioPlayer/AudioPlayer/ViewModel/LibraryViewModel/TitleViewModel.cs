using System;

using AudioPlayer.Model;

namespace AudioPlayer.ViewModel.LibraryViewModel
{
    public class TitleViewModel : ModelBase
    {
        string _fileName;
        string _name;
        uint _track;
        TimeSpan _duration;
        bool _nowPlaying;

        public string FileName
        {
            get { return _fileName; }
            set { this.SetProperty(ref _fileName, value); }
        }
        public string Name
        {
            get { return _name; }
            set { this.SetProperty(ref _name, value); }
        }
        public uint Track
        {
            get { return _track; }
            set { this.SetProperty(ref _track, value); }
        }
        public TimeSpan Duration
        {
            get { return _duration; }
            set { this.SetProperty(ref _duration, value); }
        }
        public bool NowPlaying
        {
            get { return _nowPlaying; }
            set { this.SetProperty(ref _nowPlaying, value); }
        }

        public TitleViewModel()
        {
            this.Track = 0;
            this.FileName = string.Empty;
            this.Name = string.Empty;
            this.Duration = TimeSpan.Zero;
            this.NowPlaying = false;
        }
    }
}
