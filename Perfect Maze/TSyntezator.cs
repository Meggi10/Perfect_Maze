using NAudio.Midi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_maze
{
    public class TSyntezator : IDisposable
    {
        private readonly double _attackTime = 0.05;
        private readonly double _decayTime = 0.1;
        private readonly double _sustainLevel = 0.5;
        private readonly double _releaseTime = 0.2;
        private WaveOutEvent _waveOut;
        private TSimpleMidiSynth _synth;
        private bool _disposed = false;

        public void Play(string midiFilePath)
        {
            Stop();
            _synth = new TSimpleMidiSynth(
                midiFilePath,
                _attackTime,
                _decayTime,
                _sustainLevel,
                _releaseTime
            );
            _waveOut = new WaveOutEvent();
            _waveOut.DesiredLatency = 150;
            _waveOut.Init(_synth);
            _waveOut.Play();
        }

        public void Stop()
        {
            _waveOut?.Stop();
            _waveOut?.Dispose();
            _waveOut = null;
            _synth = null;
        }

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
