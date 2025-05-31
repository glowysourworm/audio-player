using System;

using AudioPlayer.Event;

using NAudio.Wave;

namespace AudioPlayer.Component
{
    /// <summary>
    /// NAudio based Mp3 Player (see their docs)
    /// </summary>
    public class SimpleMp3Player : IDisposable
    {
        private IWavePlayer _wavePlayer;

        public event SimpleEventHandler<string> MessageEvent;

        public event SimpleEventHandler PlaybackStoppedEvent;

        public SimpleMp3Player()
        {
            _wavePlayer = null;
        }

        public void SetVolume(float volume)
        {
            _wavePlayer.Volume = Math.Clamp(volume, 0, 1);
        }

        public void Play(string fileName)
        {
            if (_wavePlayer != null)
            {
                _wavePlayer.PlaybackStopped -= OnPlaybackStopped;
                _wavePlayer.Stop();
                _wavePlayer.Dispose();
                _wavePlayer = null;
            }

            var audioFileReader = new AudioFileReader(fileName);
            audioFileReader.Volume = 1;
            _wavePlayer = new WasapiOut();
            _wavePlayer.PlaybackStopped += OnPlaybackStopped;
            _wavePlayer.Init(audioFileReader);
            _wavePlayer.Play();
        }

        public void Pause()
        {
            _wavePlayer.Pause();
        }

        public void Stop()
        {
            _wavePlayer.Stop();
        }

        public PlaybackState GetPlaybackState()
        {
            return _wavePlayer.PlaybackState;
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (this.PlaybackStoppedEvent != null)
                this.PlaybackStoppedEvent();
        }

        public void Dispose()
        {
            if (_wavePlayer != null)
            {
                _wavePlayer.Dispose();
                _wavePlayer = null;
            }
        }
    }
}
