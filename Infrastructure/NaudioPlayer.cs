using NAudio.Wave;
using System;


namespace Tikisoft.DesktopAlertsDemo.Infrastructure
{
    class NaudioPlayer : IAudioPlayer
    {
        private WaveOutEvent _waveOut;
        private Mp3FileReader _mp3Reader;
        private bool _loop;
        private bool _manuallyStopped;

        public NaudioPlayer()
        {
            _waveOut = new WaveOutEvent();
            _waveOut.PlaybackStopped += new EventHandler<StoppedEventArgs>(OnPlaybackStopped);

        }
        public bool PlayAudio(string audioFilePath, bool loop)
        {
           
            _manuallyStopped = false;
            _loop = loop;

            if (_waveOut.PlaybackState == PlaybackState.Playing)
            {
                _waveOut.Stop();
            }

            _mp3Reader = new Mp3FileReader(audioFilePath);            
            _waveOut.Init(_mp3Reader);            
            _waveOut.Play();                        
            return true;

        }

        public void StopPlaying()
        {
            if (_waveOut!=null && _waveOut.PlaybackState == PlaybackState.Playing)
            {
                _manuallyStopped = true;
                _waveOut.Stop();
            }
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            if(!_manuallyStopped & _loop)
            {
                _mp3Reader.Position = 0;                
                _waveOut.Play();
            }
           
            
        }
    }
    
}
