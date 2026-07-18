using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_maze
{
    public class TGameSoundtrack : IDisposable
    {
        private AudioFileReader FileReader;
        private WaveOutEvent Waveout;
        private TLoopStream LoopStream;
        private bool _disposed = false;
        public float Volume
        {
            get => Waveout?.Volume ?? 1f;
            set
            {
                if (Waveout != null)
                    Waveout.Volume = Math.Min(1f, Math.Max(0f, value));
            }
        }
        public void Play(string filePath, float volume = 0.5f)
        {
            FileReader = new AudioFileReader(filePath);
            LoopStream = new TLoopStream(FileReader);
            Waveout = new WaveOutEvent();
            Waveout.Init(LoopStream);
            Waveout.Volume = Math.Min(1f, Math.Max(0f, volume));
            Waveout.Play();
        }
        public void Stop()
        {
            Waveout?.Stop();
            Waveout?.Dispose();
            LoopStream?.Dispose();
            FileReader?.Dispose();
            Waveout = null;
            LoopStream = null;
            FileReader = null;
        }
        public void Pause() => Waveout?.Pause();
        public void Resume() => Waveout?.Play();
        public void Dispose()
        {
            if (!_disposed)
            {
                Stop();
                _disposed = true;
            }
        }
    }
}
