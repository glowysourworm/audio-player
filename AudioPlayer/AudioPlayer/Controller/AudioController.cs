using AudioPlayer.Component;
using AudioPlayer.Controller.Interface;
using AudioPlayer.Event;

using NAudio.Wave;

namespace AudioPlayer.Controller
{
    public class AudioController : IAudioController
    {
        public event SimpleEventHandler PlaybackStoppedEvent;

        private SimpleMp3Player _player;

        public AudioController()
        {
            _player = new SimpleMp3Player();
            _player.PlaybackStoppedEvent += PlaybackStoppedEvent;
        }

        public PlaybackState GetPlaybackState()
        {
            return _player.GetPlaybackState();
        }

        public void Pause()
        {
            _player.Pause();
        }

        public void Play(string fileName)
        {
            _player.Play(fileName);
        }

        public void Stop()
        {
            _player.Stop();
        }
    }
}
